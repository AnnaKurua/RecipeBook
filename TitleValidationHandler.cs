namespace RecipeBook
{
    public class TitleValidationHandler : AbstractValidationHandler
    {
        protected override ValidationResult DoValidate(Recipe recipe)
        {
            if (string.IsNullOrWhiteSpace(recipe.Title))
            {
                return ValidationResult.Failure("Recipe title is required.");
            }

            return ValidationResult.Success();
        }
    }
}
