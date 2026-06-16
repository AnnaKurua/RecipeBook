using System;

namespace RecipeBook
{
    /// <summary>
    /// PATTERN: Chain of Responsibility (chain assembly)
    /// Builds the chain: Title → Ingredients → Amounts.
    /// Add new handlers here without touching existing code.
    /// </summary>
    public static class RecipeValidationChain
    {
        public static IValidationHandler Build()
        {
            var title = new TitleValidationHandler();
            var ingredients = new IngredientsValidationHandler();
            var amounts = new AmountValidationHandler();

            // FIX: Added the ?. operator to safely navigate nullable returns
            title.SetNext(ingredients)
                 ?.SetNext(amounts);

            return title;
        }
    }
}