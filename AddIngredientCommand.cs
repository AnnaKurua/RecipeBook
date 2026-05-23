namespace RecipeBook
{
    public class AddIngredientCommand : IShoppingListCommand
    {
        private readonly ShoppingList list;
        private readonly Ingredient ingredient;

        public AddIngredientCommand(ShoppingList list, Ingredient ingredient)
        {
            this.list = list;
            this.ingredient = ingredient;
        }

        public string Description => "Add manual item to shopping list";

        public void Execute()
        {
            list.AddItem(ingredient);
        }
    }
}
