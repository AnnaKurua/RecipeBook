namespace RecipeBook
{
    public class ValidationResult
    {
        public bool IsValid { get; init; }
        public string Message { get; init; } = string.Empty;

        public static ValidationResult Success() => new() { IsValid = true };

        public static ValidationResult Failure(string message) =>
            new() { IsValid = false, Message = message };
    }
}
