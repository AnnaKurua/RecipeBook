namespace RecipeBook
{
    // Defines the required data operations for recipes
    // Any class that stores/retrieves recipes must implement this;
    public interface IRecipeRepo
    {
        List<Recipe> GetAll();
        Recipe?      GetById(Guid id);
        void         Save(Recipe recipe);
        void         Delete(Guid id);
    }

    // Defines the required data operations for users
    // Any class that stores/retrieves users must implement this;
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
