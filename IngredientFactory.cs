using System;

namespace RecipeBook
{
    /// <summary>
    /// PATTERN: Factory
    /// All ingredient creation is centralized here.
    /// Handles trimming, default values, and category validation.
    /// </summary>
    public static class IngredientFactory
    {
        // Allowed categories
        private static readonly string[] ValidCategories =
        {
            "Dairy",
            "Meat",
            "Fruit",
            "Vegetable",
            "Grain",
            "Spices",
            "General"
        };

        public static Ingredient Create(
            string name,
            double amount,
            string unit,
            string category)
        {
            string cleanedCategory =
                string.IsNullOrWhiteSpace(category)
                    ? "General"
                    : category.Trim();

            // Validate category
            if (!ValidCategories.Contains(cleanedCategory,
                StringComparer.OrdinalIgnoreCase))
            {
                cleanedCategory = "General";
            }

            return new Ingredient(
                name.Trim(),
                amount,
                unit.Trim(),
                cleanedCategory);
        }

        /// <summary>
        /// Reads one ingredient from the console.
        /// </summary>
        public static Ingredient? CreateFromConsole()
        {
            Console.Write("  Ingredient name (blank to finish): ");
            string? name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
                return null;

            Console.Write("  Amount: ");
            if (!double.TryParse(Console.ReadLine(), out double amount)
                || amount <= 0)
            {
                Console.WriteLine("  Invalid amount — ingredient skipped.");
                return null;
            }

            Console.Write("  Unit (e.g. grams, ml): ");
            string unit = Console.ReadLine() ?? "unit";

            Console.WriteLine("  Categories:");
            Console.WriteLine("  Dairy, Meat, Fruit, Vegetable, Grain, Spices");
            Console.Write("  Category [General]: ");

            string category = Console.ReadLine() ?? "General";

            return Create(name, amount, unit, category);
        }
    }
}