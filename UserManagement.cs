namespace RecipeBook
{
    public class UserManagement
    {
        private readonly IUserRepo userRepo;
        private readonly IRecipeRepo recipeRepo;

        public UserManagement(IUserRepo userRepo, IRecipeRepo recipeRepo)
        {
            this.userRepo = userRepo;
            this.recipeRepo = recipeRepo;
        }

        public User RegisterUser(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name is required.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            {
                throw new ArgumentException("A valid email is required.", nameof(email));
            }

            if (userRepo.GetByEmail(email) != null)
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }

            var user = new User(name.Trim(), email.Trim());
            userRepo.Add(user);
            return user;
        }

        public User? GetProfile(Guid id)
        {
            return userRepo.GetUser(id);
        }

        public void DeleteUser(Guid id)
        {
            userRepo.Delete(id);
        }

        public void UpdateEmail(Guid id, string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail) || !newEmail.Contains('@'))
            {
                throw new ArgumentException("A valid email is required.", nameof(newEmail));
            }

            User? user = userRepo.GetUser(id);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            user.Email = newEmail.Trim();
            userRepo.UpdateUser(user);
        }

        public void SaveUserState(User user)
        {
            userRepo.UpdateUser(user);
        }

        public List<User> ListUsers()
        {
            return userRepo.GetAll();
        }

        public User? LoginByEmail(string email)
        {
            return userRepo.GetByEmail(email);
        }
    }
}
