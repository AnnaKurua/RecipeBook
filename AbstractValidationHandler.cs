namespace RecipeBook
{
    public abstract class AbstractValidationHandler : IValidationHandler
    {
        private IValidationHandler? next;

        public IValidationHandler? SetNext(IValidationHandler handler)
        {
            next = handler;
            return handler;
        }

        public ValidationResult Validate(Recipe recipe)
        {
            ValidationResult result = DoValidate(recipe);
            if (!result.IsValid)
            {
                return result;
            }

            return next?.Validate(recipe) ?? ValidationResult.Success();
        }

        protected abstract ValidationResult DoValidate(Recipe recipe);
    }
}
