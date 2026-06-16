namespace RecipeBook
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public static UserDto From(User user) => new()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
