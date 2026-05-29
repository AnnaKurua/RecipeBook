using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeBook
{
    public class User
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // FIX: Protected backing list containers
        private readonly List<Recipe> _userRecipes = new();

        public IReadOnlyList<Recipe> UserRecipes => _userRecipes;

        // Expose Shopping list as a secure getter property
        public ShoppingList UserShopList { get; private set; } = new();

        // Parameterless constructor required for JSON deserialization
        public User() { }

        public User(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public IReadOnlyList<Recipe> GetSavedRecipes() => _userRecipes;

        public void AddSavedRecipe(Recipe recipe)
        {
            if (recipe == null) throw new ArgumentNullException(nameof(recipe));

            if (!_userRecipes.Any(r => r.Id == recipe.Id))
                _userRecipes.Add(recipe);
        }

        public bool HasSavedRecipe(Guid recipeId) =>
            _userRecipes.Any(r => r.Id == recipeId);
    }
}