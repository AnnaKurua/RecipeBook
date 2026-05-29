using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeBook
{
    public class JsonRecipeRepo : IRecipeRepo
    {
        private readonly string _filePath;
        private readonly List<Recipe> _recipes;

        public JsonRecipeRepo()
        {
            _filePath = AppSettings.Instance.RecipesFile;
            _recipes = JsonFileStore.LoadList<Recipe>(_filePath);
        }

        public List<Recipe> GetAll() => _recipes;
        public Recipe? GetById(Guid id) => _recipes.FirstOrDefault(r => r.Id == id);

        public void Save(Recipe recipe)
        {
            if (recipe == null) return;

            int index = _recipes.FindIndex(r => r.Id == recipe.Id);
            if (index >= 0) _recipes[index] = recipe;
            else _recipes.Add(recipe);
            Persist();
        }

        public void Delete(Guid id)
        {
            _recipes.RemoveAll(r => r.Id == id);
            Persist();
        }

        private void Persist() => JsonFileStore.SaveList(_filePath, _recipes);
    }
}