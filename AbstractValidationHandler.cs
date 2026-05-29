namespace RecipeBook
{
    /// <summary>
    /// PATTERN: Chain of Responsibility (abstract base)
    /// Handles the "pass to next" logic so concrete handlers only implement their own check.
    /// </summary>
    public abstract class AbstractValidationHandler : IValidationHandler
    {
        private IValidationHandler? _next;

        public IValidationHandler? SetNext(IValidationHandler handler)
        {
            _next = handler;
            return handler;
        }

        public ValidationResult Validate(Recipe recipe)
        {
            ValidationResult result = DoValidate(recipe);
            if (!result.IsValid)
                return result;

            return _next?.Validate(recipe) ?? ValidationResult.Success();
        }

        protected abstract ValidationResult DoValidate(Recipe recipe);
    }
}
