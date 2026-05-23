namespace RecipeBook
{
    public class RecipeService
    {
        private readonly IRecipeRepo repository;
        private readonly RecipeManager recipeManager;

        public RecipeService(IRecipeRepo repository, RecipeManager recipeManager)
        {
            this.repository = repository;
            this.recipeManager = recipeManager;
        }

        public List<Recipe> BrowseAllRecipes()
        {
            return repository.GetAll();
        }

        public Recipe? GetRecipeDetails(Guid id)
        {
            return repository.GetById(id);
        }

        public Recipe? GetScaledRecipe(Guid id, int targetServings)
        {
            Recipe? recipe = repository.GetById(id);
            if (recipe == null)
            {
                return null;
            }

            Recipe scaled = recipe.Clone();
            scaled.Scale(targetServings);
            return scaled;
        }

        public void CreateRecipe(Recipe recipe)
        {
            repository.Save(recipe);
            recipeManager.Notify($"Recipe '{recipe.Title}' was created.");
        }

        public void DeleteRecipe(Guid id)
        {
            Recipe? recipe = repository.GetById(id);
            repository.Delete(id);
            recipeManager.Notify(recipe == null
                ? "A recipe was deleted."
                : $"Recipe '{recipe.Title}' was deleted.");
        }
    }
}
