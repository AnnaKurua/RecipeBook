using System;
using System.Collections.Generic;

namespace RecipeBook
{
    public class RecipeService
    {
        private IRecipeRepo repository;

        public RecipeService(IRecipeRepo repository)
        {
            this.repository = repository;
        }

        public void AddRecipe(Recipe recipe)
        {
            repository.Save(recipe);
        }

        public void Delete(Guid id)
        {
            repository.Delete(id);
        }

        public Recipe GetById(Guid id)
        {
            return repository.GetById(id);
        }

        public List<Recipe> GetAll()
        {
            return repository.GetAll();
        }
    }
}