namespace RecipeBook
{
    /// <summary>
    /// Chain of Responsibility — each handler validates one concern, then passes to the next.
    /// </summary>
    public interface IValidationHandler
    {
        IValidationHandler? SetNext(IValidationHandler handler);
        ValidationResult Validate(Recipe recipe);
    }
}
