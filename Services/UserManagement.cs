using System;
using System.Collections.Generic;

namespace RecipeBook
{
    public class UserManagement
    {
        private readonly IUserRepo _userRepo;
        private readonly IRecipeRepo _recipeRepo;

        public UserManagement(IUserRepo userRepo, IRecipeRepo recipeRepo)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            _recipeRepo = recipeRepo ?? throw new ArgumentNullException(nameof(recipeRepo));
        }

        public User RegisterUser(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));

            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
                throw new ArgumentException("A valid email is required.", nameof(email));

            if (_userRepo.GetByEmail(email) != null)
                throw new InvalidOperationException("A user with this email already exists.");

            var user = new User(name.Trim(), email.Trim());
            _userRepo.Add(user);
            return user;
        }

        public User? LoginByEmail(string email) =>
            _userRepo.GetByEmail(email);

        public void UpdateEmail(Guid id, string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail) || !newEmail.Contains('@'))
                throw new ArgumentException("A valid email is required.", nameof(newEmail));

            User? user = _userRepo.GetUser(id)
                ?? throw new InvalidOperationException("User not found.");

            user.Email = newEmail.Trim();
            _userRepo.UpdateUser(user);
        }

        public void SaveUserState(User user) =>
            _userRepo.UpdateUser(user);

        public List<User> ListUsers() => _userRepo.GetAll();
    }
}