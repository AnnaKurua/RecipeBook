namespace RecipeBook
{
    /// <summary>
    /// PATTERN: Strategy
    /// Defines the sorting contract. Swap implementations at runtime
    /// without changing the ShoppingList class.
    /// </summary>
    public interface ISortStrategy
    {
        List<Ingredient> Sort(List<Ingredient> items);
    }

    // ── Concrete strategy 1: sort by ingredient name ─────────────────────────
    public class AlphabeticalSortStrategy : ISortStrategy
    {
        public List<Ingredient> Sort(List<Ingredient> items) =>
            items.OrderBy(i => i.Name).ToList();
    }

    // ── Concrete strategy 2: sort by category then name ──────────────────────
    public class CategorySortStrategy : ISortStrategy
    {
        public List<Ingredient> Sort(List<Ingredient> items) =>
            items.OrderBy(i => i.Category).ThenBy(i => i.Name).ToList();
    }
}
