using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeBook
{
    // Recipe.cs
    public class Recipe
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public List<string> Steps { get; set; } = new List<string>();

        public Recipe(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public void AddIngredient(Ingredient ingredient)
        {
            Ingredients.Add(ingredient);
        }

        public void RemoveIngredient(Guid ingredientId)
        {
            Ingredients.RemoveAll(i => i.Id == ingredientId);
        }

        public void Scale(int servings)
        {
            // Basic scaling — assumes recipe is currently for 1 serving
            foreach (var ingredient in Ingredients)
            {
                ingredient.Amount *= servings;
            }
        }
    }
}
