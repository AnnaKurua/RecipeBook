using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeBook
{
    public class JsonUserRepo : IUserRepo
    {
        private readonly string _filePath;
        private readonly List<User> _users;

        public JsonUserRepo()
        {
            _filePath = AppSettings.Instance.UsersFile;
            _users = JsonFileStore.LoadList<User>(_filePath);
        }

        public User? GetUser(Guid id) => _users.FirstOrDefault(u => u.Id == id);
        public List<User> GetAll() => _users;

        public User? GetByEmail(string email) =>
            _users.FirstOrDefault(u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));

        public void Add(User user)
        {
            if (user == null) return;
            _users.Add(user);
            Persist();
        }

        public void Delete(Guid id)
        {
            _users.RemoveAll(u => u.Id == id);
            Persist();
        }

        public void UpdateUser(User user)
        {
            if (user == null) return;
            int index = _users.FindIndex(u => u.Id == user.Id);
            if (index >= 0)
            {
                _users[index] = user;
                Persist();
            }
        }

        private void Persist() => JsonFileStore.SaveList(_filePath, _users);
    }
}