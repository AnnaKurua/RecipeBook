using System;
using System.Collections.Generic;

namespace RecipeBook
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }

        private List<Recipe> savedRecipes = new List<Recipe>();

        public User(string name)
        {
            Name = name;
        }

        public List<Recipe> GetSavedRecipes()
        {
            return savedRecipes;
        }
    }
}