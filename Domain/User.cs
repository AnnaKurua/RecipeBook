using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeBook
{
    public class User
    {
        [BsonId]
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public List<Recipe> UserRecipes { get; private set; } = new();

        // Expose Shopping list as a secure getter property
        public ShoppingList UserShopList { get; private set; } = new();

        // Parameterless constructor required for JSON deserialization
        public User() { }

        public User(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public IReadOnlyList<Recipe> GetSavedRecipes() => UserRecipes;

        public void AddSavedRecipe(Recipe recipe)
        {
            if (recipe == null) throw new ArgumentNullException(nameof(recipe));

            if (!UserRecipes.Any(r => r.Id == recipe.Id))
                UserRecipes.Add(recipe);
        }

        public bool HasSavedRecipe(Guid recipeId) =>
            UserRecipes.Any(r => r.Id == recipeId);
    }
}