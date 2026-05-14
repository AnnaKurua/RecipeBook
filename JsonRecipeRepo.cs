using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeBook
{
    public class JsonRecipeRepo : IRecipeRepo
    {
        private List<Recipe> recipes = new List<Recipe>();

        public List<Recipe> GetAll()
        {
            return recipes;
        }

        public Recipe GetById(Guid id)
        {
            return recipes.FirstOrDefault(r => r.Id == id);
        }

        public void Save(Recipe r)
        {
            recipes.Add(r);
        }

        public void Delete(Guid id)
        {
            recipes.RemoveAll(r => r.Id == id);
        }
    }
}