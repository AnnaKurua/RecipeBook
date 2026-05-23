namespace RecipeBook
{
    public static class RecipeValidationChain
    {
        public static IValidationHandler Build()
        {
            var title = new TitleValidationHandler();
            var ingredients = new IngredientsValidationHandler();
            var amounts = new AmountValidationHandler();

            ingredients.SetNext(amounts);
            title.SetNext(ingredients);
            return title;
        }
    }
}
