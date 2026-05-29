namespace RecipeBook
{
    /// <summary>
    /// PATTERN: Factory
    /// All ingredient-creation logic lives here: trimming, default values,
    /// console prompting. Callers never call `new Ingredient(...)` directly.
    /// </summary>
    public static class IngredientFactory
    {
        public static Ingredient Create(string name, double amount, string unit, string category) =>
            new Ingredient(
                name.Trim(),
                amount,
                unit.Trim(),
                string.IsNullOrWhiteSpace(category) ? "General" : category.Trim());

        /// <summary>
        /// Interactively reads one ingredient from the console.
        /// Returns null if the user enters an empty name (signals "done").
        /// </summary>
        public static Ingredient? CreateFromConsole()
        {
            Console.Write("  Ingredient name (blank to finish): ");
            string? name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
                return null;

            Console.Write("  Amount: ");
            if (!double.TryParse(Console.ReadLine(), out double amount) || amount <= 0)
            {
                Console.WriteLine("  Invalid amount — ingredient skipped.");
                return null;
            }

            Console.Write("  Unit (e.g. grams, ml): ");
            string unit = Console.ReadLine() ?? "unit";

            Console.Write("  Category (e.g. Dairy, Meat) [General]: ");
            string category = Console.ReadLine() ?? "General";

            return Create(name, amount, unit, category);
        }
    }
}
