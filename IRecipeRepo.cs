namespace RecipeBook
{
    public interface IRecipeRepo
    {
        List<Recipe> GetAll();
        Recipe? GetById(Guid id);
        void Save(Recipe recipe);
        void Delete(Guid id);
    }
}
