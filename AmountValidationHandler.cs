namespace RecipeBook
{
    public class AmountValidationHandler : AbstractValidationHandler
    {
        protected override ValidationResult DoValidate(Recipe recipe)
        {
            foreach (var ingredient in recipe.Ingredients)
            {
                if (ingredient.Amount <= 0)
                {
                    return ValidationResult.Failure(
                        $"Ingredient '{ingredient.Name}' must have an amount greater than zero.");
                }

                if (string.IsNullOrWhiteSpace(ingredient.Name))
                {
                    return ValidationResult.Failure("Every ingredient must have a name.");
                }
            }

            return ValidationResult.Success();
        }
    }
}
