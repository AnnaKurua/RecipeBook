namespace RecipeBook
{
    public class JsonUserRepo : IUserRepo
    {
        private readonly string filePath;
        private List<User> users;

        public JsonUserRepo()
        {
            filePath = AppSettings.Instance.UsersFile;
            users = JsonFileStore.LoadList<User>(filePath);
        }

        public User? GetUser(Guid id)
        {
            return users.FirstOrDefault(u => u.Id == id);
        }

        public User? GetByEmail(string email)
        {
            return users.FirstOrDefault(u =>
                string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));
        }

        public void Add(User user)
        {
            users.Add(user);
            Persist();
        }

        public void Delete(Guid id)
        {
            users.RemoveAll(u => u.Id == id);
            Persist();
        }

        public void UpdateUser(User user)
        {
            int index = users.FindIndex(u => u.Id == user.Id);
            if (index >= 0)
            {
                users[index] = user;
                Persist();
            }
        }

        public List<User> GetAll()
        {
            return users;
        }

        private void Persist()
        {
            JsonFileStore.SaveList(filePath, users);
        }
    }
}
