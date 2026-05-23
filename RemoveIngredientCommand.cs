namespace RecipeBook
{
    public class RemoveIngredientCommand : IShoppingListCommand
    {
        private readonly ShoppingList list;
        private readonly Guid ingredientId;

        public RemoveIngredientCommand(ShoppingList list, Guid ingredientId)
        {
            this.list = list;
            this.ingredientId = ingredientId;
        }

        public string Description => "Remove bought ingredient from shopping list";

        public void Execute()
        {
            list.RemoveIngredient(ingredientId);
        }
    }
}
