namespace RecipeBook
{
    /// <summary>Repository abstraction for recipes.</summary>
    public interface IRecipeRepo
    {
        List<Recipe> GetAll();
        Recipe?      GetById(Guid id);
        void         Save(Recipe recipe);
        void         Delete(Guid id);
    }

    /// <summary>Repository abstraction for users.</summary>
    public interface IUserRepo
    {
        User?      GetUser(Guid id);
        User?      GetByEmail(string email);
        void       Add(User user);
        void       Delete(Guid id);
        void       UpdateUser(User user);
        List<User> GetAll();
    }
}
