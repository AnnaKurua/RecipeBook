namespace RecipeBook
{
    public class JsonRecipeRepo : IRecipeRepo
    {
        private readonly string filePath;
        private List<Recipe> recipes;

        public JsonRecipeRepo()
        {
            filePath = AppSettings.Instance.RecipesFile;
            recipes = JsonFileStore.LoadList<Recipe>(filePath);
        }

        public List<Recipe> GetAll()
        {
            return recipes;
        }

        public Recipe? GetById(Guid id)
        {
            return recipes.FirstOrDefault(r => r.Id == id);
        }

        public void Save(Recipe recipe)
        {
            int index = recipes.FindIndex(r => r.Id == recipe.Id);
            if (index >= 0)
            {
                recipes[index] = recipe;
            }
            else
            {
                recipes.Add(recipe);
            }

            Persist();
        }

        public void Delete(Guid id)
        {
            recipes.RemoveAll(r => r.Id == id);
            Persist();
        }

        private void Persist()
        {
            JsonFileStore.SaveList(filePath, recipes);
        }
    }
}
