namespace RecipeBook
{
    /// <summary>
    /// PATTERN: Chain of Responsibility
    /// Each handler validates one concern, then passes to the next in the chain.
    /// </summary>
    public interface IValidationHandler
    {
        IValidationHandler? SetNext(IValidationHandler handler);
        ValidationResult Validate(Recipe recipe);
    }
}
