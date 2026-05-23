namespace RecipeBook
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<Recipe> UserRecipes { get; set; } = new();
        public ShoppingList UserShopList { get; set; } = new();

        public User()
        {
        }

        public User(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public List<Recipe> GetSavedRecipes()
        {
            return UserRecipes;
        }

        public void AddSavedRecipe(Recipe recipe)
        {
            if (UserRecipes.Any(r => r.Id == recipe.Id))
            {
                return;
            }

            UserRecipes.Add(recipe);
        }
    }
}
