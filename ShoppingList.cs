namespace RecipeBook
{
    public class ShoppingList : ISubscriber
    {
        public List<Ingredient> Items { get; set; } = new();

        private ISortStrategy? sortStrategy;

        public void AddRecipeIngredients(Recipe recipe)
        {
            foreach (var ingredient in recipe.Ingredients)
            {
                Items.Add(ingredient.Clone());
            }
        }

        public void AddItem(Ingredient ingredient)
        {
            Items.Add(ingredient);
        }

        public void RemoveIngredient(Guid itemId)
        {
            Items.RemoveAll(i => i.Id == itemId);
        }

        public void RemoveMatching(string name, string unit)
        {
            Items.RemoveAll(i =>
                string.Equals(i.Name, name, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(i.Unit, unit, StringComparison.OrdinalIgnoreCase));
        }

        public void Clear()
        {
            Items.Clear();
        }

        public List<Ingredient> GetConsolidatedList()
        {
            return Items
                .GroupBy(i => new { i.Name, i.Unit, i.Category })
                .Select(group => new Ingredient(
                    group.Key.Name,
                    group.Sum(i => i.Amount),
                    group.Key.Unit,
                    group.Key.Category))
                .ToList();
        }

        public void SetSortStrategy(ISortStrategy strategy)
        {
            sortStrategy = strategy;
        }

        public List<Ingredient> GetSortedList()
        {
            var source = GetConsolidatedList();
            return sortStrategy == null ? source : sortStrategy.Sort(source);
        }

        public void Update(object data)
        {
            Console.WriteLine($"[Shopping list notified] {data}");
        }

        public void Display(bool consolidated = true, bool sorted = false)
        {
            List<Ingredient> toShow = consolidated ? GetConsolidatedList() : new List<Ingredient>(Items);
            if (sorted && sortStrategy != null)
            {
                toShow = sortStrategy.Sort(toShow);
            }

            if (toShow.Count == 0)
            {
                Console.WriteLine("  (empty)");
                return;
            }

            for (int i = 0; i < toShow.Count; i++)
            {
                var item = toShow[i];
                Console.WriteLine($"  {i + 1}. [{item.Category}] {item.Amount} {item.Unit} {item.Name}");
            }
        }
    }
}
