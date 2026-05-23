namespace RecipeBook
{
    public interface IUserRepo
    {
        User? GetUser(Guid id);
        User? GetByEmail(string email);
        void Add(User user);
        void Delete(Guid id);
        void UpdateUser(User user);
        List<User> GetAll();
    }
}
