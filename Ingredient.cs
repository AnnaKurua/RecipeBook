using System;

namespace RecipeBook
{
    public class Ingredient
    {
        // Enforcing encapsulation: Id can only be set inside this class
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;

        // Parameterless constructor required for JSON deserialization/database frameworks
        public Ingredient() { }

        public Ingredient(string name, double amount, string unit, string category)
        {
            Name = name;
            Amount = amount >= 0 ? amount : 0; // Guard clause to prevent negative quantities
            Unit = unit;
            Category = category;
        }

        /// <summary>
        /// PATTERN: Prototype
        /// Returns a deep copy of the ingredient.
        /// </summary>
        /// <param name="createNewId">
        /// If true, generates a brand new GUID (for "Save Copy" functionality).
        /// If false, keeps the original GUID (for temporary calculations like scaling).
        /// </param>
        public Ingredient Clone(bool createNewId = false)
        {
            return new Ingredient(Name, Amount, Unit, Category)
            {
                // If createNewId is true, assign a fresh Guid. Otherwise, pass the original identity forward.
                Id = createNewId ? Guid.NewGuid() : this.Id
            };
        }
    }
}