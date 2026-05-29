namespace RecipeBook
{
    /// <summary>
    /// PATTERN: Command
    /// Encapsulates a shopping-list modification as an object.
    /// Makes it easy to add undo/redo or a command history later.
    /// </summary>
    public interface IShoppingListCommand
    {
        string Description { get; }
        void Execute();
    }

    // ── Command 1: add an ingredient to the list ─────────────────────────────
    public class AddIngredientCommand : IShoppingListCommand
    {
        private readonly ShoppingList _list;
        private readonly Ingredient   _ingredient;

        public AddIngredientCommand(ShoppingList list, Ingredient ingredient)
        {
            _list       = list;
            _ingredient = ingredient;
        }

        public string Description => $"Add '{_ingredient.Name}' to shopping list";

        public void Execute() => _list.AddItem(_ingredient);
    }

    // ── Command 2: remove an ingredient from the list by Id ──────────────────
    public class RemoveIngredientCommand : IShoppingListCommand
    {
        private readonly ShoppingList _list;
        private readonly string       _name;
        private readonly string       _unit;

        public RemoveIngredientCommand(ShoppingList list, string name, string unit)
        {
            _list = list;
            _name = name;
            _unit = unit;
        }

        public string Description => $"Remove '{_name}' from shopping list";

        public void Execute() => _list.RemoveMatching(_name, _unit);
    }
}
