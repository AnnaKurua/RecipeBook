namespace RecipeBook
{
    /// <summary>
    /// Factory — centralizes creation of Ingredient instances from user input.
    /// </summary>
    public static class IngredientFactory
    {
        public static Ingredient Create(string name, double amount, string unit, string category)
        {
            return new Ingredient(
                name.Trim(),
                amount,
                unit.Trim(),
                string.IsNullOrWhiteSpace(category) ? "General" : category.Trim());
        }

        public static Ingredient? CreateFromConsole()
        {
            Console.Write("Ingredient name: ");
            string? name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            Console.Write("Amount: ");
            if (!double.TryParse(Console.ReadLine(), out double amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount.");
                return null;
            }

            Console.Write("Unit (e.g. grams, ml): ");
            string unit = Console.ReadLine() ?? "unit";

            Console.Write("Category (e.g. Dairy, Meat): ");
            string category = Console.ReadLine() ?? "General";

            return Create(name, amount, unit, category);
        }
    }
}
