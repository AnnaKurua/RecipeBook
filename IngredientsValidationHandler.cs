namespace RecipeBook
{
    public class IngredientsValidationHandler : AbstractValidationHandler
    {
        protected override ValidationResult DoValidate(Recipe recipe)
        {
            if (recipe.Ingredients == null || recipe.Ingredients.Count == 0)
            {
                return ValidationResult.Failure("Recipe must contain at least one ingredient.");
            }

            return ValidationResult.Success();
        }
    }
}
