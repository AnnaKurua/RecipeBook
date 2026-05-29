namespace RecipeBook
{
    // ── Handler 1: checks that the recipe has a non-empty title ──────────────
    public class TitleValidationHandler : AbstractValidationHandler
    {
        protected override ValidationResult DoValidate(Recipe recipe)
        {
            if (string.IsNullOrWhiteSpace(recipe.Title))
                return ValidationResult.Failure("Recipe title is required.");

            return ValidationResult.Success();
        }
    }

    // ── Handler 2: checks that at least one ingredient exists ────────────────
    public class IngredientsValidationHandler : AbstractValidationHandler
    {
        protected override ValidationResult DoValidate(Recipe recipe)
        {
            if (recipe.Ingredients == null || recipe.Ingredients.Count == 0)
                return ValidationResult.Failure("Recipe must contain at least one ingredient.");

            return ValidationResult.Success();
        }
    }

    // ── Handler 3: checks every ingredient has a name and a positive amount ──
    public class AmountValidationHandler : AbstractValidationHandler
    {
        protected override ValidationResult DoValidate(Recipe recipe)
        {
            foreach (var ingredient in recipe.Ingredients)
            {
                if (string.IsNullOrWhiteSpace(ingredient.Name))
                    return ValidationResult.Failure("Every ingredient must have a name.");

                if (ingredient.Amount <= 0)
                    return ValidationResult.Failure(
                        $"Ingredient '{ingredient.Name}' must have an amount greater than zero.");
            }

            return ValidationResult.Success();
        }
    }
}
