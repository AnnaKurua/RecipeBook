using System;
using System.Linq;

namespace RecipeBook
{
    public interface IShoppingListCommand
    {
        string Description { get; }
        void Execute();
        void Undo();
    }
    public class AddIngredientCommand : IShoppingListCommand
    {
        private readonly ShoppingList _list;
        private readonly Ingredient _ingredient;

        public AddIngredientCommand(ShoppingList list, Ingredient ingredient)
        {
            _list = list;
            _ingredient = ingredient;
        }

        public string Description => $"Add '{_ingredient.Name}' to shopping list";

        public void Execute()
        {
            _list.AddItem(_ingredient);
        }

        public void Undo()
        {
            _list.RemoveMatching(_ingredient.Name, _ingredient.Unit);
        }
    }
    public class RemoveIngredientCommand : IShoppingListCommand
    {
        private readonly ShoppingList _list;
        private readonly string _name;
        private readonly string _unit;
        private Ingredient? _backup;

        public RemoveIngredientCommand(ShoppingList list, string name, string unit)
        {
            _list = list;
            _name = name;
            _unit = unit;
        }

        public string Description => $"Remove '{_name}' from shopping list";

        public void Execute()
        {
            _backup = _list.Items.FirstOrDefault(i =>
                i.Name.Equals(_name, StringComparison.OrdinalIgnoreCase) &&
                i.Unit.Equals(_unit, StringComparison.OrdinalIgnoreCase));

            _list.RemoveMatching(_name, _unit);
        }

        public void Undo()
        {
            if (_backup != null)
            {
                _list.AddItem(_backup);
            }
        }
    }
}
