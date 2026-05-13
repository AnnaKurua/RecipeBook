using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeBook
{
    // Ingredient.cs
    public class Ingredient
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public double Amount { get; set; }
        public string Unit { get; set; }
        public string Category { get; set; }

        public Ingredient(string name, double amount, string unit, string category)
        {
            Name = name;
            Amount = amount;
            Unit = unit;
            Category = category;
        }
    }
}
