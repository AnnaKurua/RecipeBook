namespace RecipeBook
{
    public class Ingredient
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;

        public Ingredient()
        {
        }

        public Ingredient(string name, double amount, string unit, string category)
        {
            Name = name;
            Amount = amount;
            Unit = unit;
            Category = category;
        }

        public Ingredient Clone()
        {
            return new Ingredient(Name, Amount, Unit, Category)
            {
                Id = Guid.NewGuid()
            };
        }
    }
}
