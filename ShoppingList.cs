using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeBook
{
    /// <summary>
    /// PATTERN: Observer (subscriber) — reacts to recipe-change events.
    /// PATTERN: Strategy              — delegates sorting to ISortStrategy.
    /// </summary>
    public class ShoppingList : ISubscriber
    {
        // FIX: Encapsulated the collection so external files cannot type: shoppingList.Items = null;
        private readonly List<Ingredient> _items = new();

        // Exposing it safely as a Read-Only list for UI binding or verification
        public IReadOnlyList<Ingredient> Items => _items;

        private ISortStrategy? _sortStrategy;

        // ── Adding ────────────────────────────────────────────────────────────

        public void AddRecipeIngredients(Recipe recipe)
        {
            if (recipe == null) throw new ArgumentNullException(nameof(recipe));

            foreach (var ingredient in recipe.Ingredients)
            {
                // We pass 'false' to Clone because a shopping list item needs to retain 
                // the ID link to know which recipe components it maps back to.
                _items.Add(ingredient.Clone(createNewId: false));
            }
        }

        public void AddItem(Ingredient ingredient)
        {
            if (ingredient == null) throw new ArgumentNullException(nameof(ingredient));
            _items.Add(ingredient);
        }

        // ── Removing ──────────────────────────────────────────────────────────

        public void RemoveMatching(string name, string unit) =>
            _items.RemoveAll(i =>
                string.Equals(i.Name, name, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(i.Unit, unit, StringComparison.OrdinalIgnoreCase));

        public void Clear() => _items.Clear();

        // ── Observer: react to recipe changes ─────────────────────────────────

        /// <summary>
        /// Called by RecipeManager when a recipe the user has saved changes or is deleted.
        /// - Updated: re-syncs the ingredients from the new version of the recipe.
        /// - Deleted: removes all ingredients that came from that recipe.
        /// </summary>
        public void OnRecipeChanged(RecipeChangedEvent evt)
        {
            if (evt == null || evt.Recipe == null) return;

            switch (evt.Kind)
            {
                case RecipeChangeKind.Deleted:
                    // Remove every ingredient whose name+unit matches one in the deleted recipe
                    foreach (var ing in evt.Recipe.Ingredients)
                        RemoveMatching(ing.Name, ing.Unit);

                    Console.WriteLine($"[Shopping list] Ingredients removed: recipe '{evt.Recipe.Title}' was deleted.");
                    break;

                case RecipeChangeKind.Updated:
                    // Drop the old ingredients, add the new ones
                    foreach (var ing in evt.Recipe.Ingredients)
                        RemoveMatching(ing.Name, ing.Unit);

                    AddRecipeIngredients(evt.Recipe);

                    Console.WriteLine($"[Shopping list] Ingredients refreshed: recipe '{evt.Recipe.Title}' was updated.");
                    break;
            }
        }

        // ── Sorting (Strategy) ────────────────────────────────────────────────

        public void SetSortStrategy(ISortStrategy strategy) => _sortStrategy = strategy;

        // ── Read helpers ──────────────────────────────────────────────────────

        /// <summary>
        /// Merges duplicate name+unit rows by summing their amounts.
        /// </summary>
        public List<Ingredient> GetConsolidatedList() =>
            _items
                .GroupBy(i => new { i.Name, i.Unit, i.Category })
                .Select(g => new Ingredient(g.Key.Name, g.Sum(i => i.Amount), g.Key.Unit, g.Key.Category))
                .ToList();

        public List<Ingredient> GetSortedList()
        {
            var source = GetConsolidatedList();
            return _sortStrategy == null ? source : _sortStrategy.Sort(source);
        }

        public void Display(bool consolidated = true, bool sorted = false)
        {
            List<Ingredient> toShow = consolidated ? GetConsolidatedList() : new List<Ingredient>(_items);
            if (sorted && _sortStrategy != null)
                toShow = _sortStrategy.Sort(toShow);

            if (toShow.Count == 0) { Console.WriteLine("  (empty)"); return; }

            for (int i = 0; i < toShow.Count; i++)
            {
                var item = toShow[i];
                Console.WriteLine($"  {i + 1}. [{item.Category}] {item.Amount:G} {item.Unit} {item.Name}");
            }
        }
    }
}