using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeBook
{
    // ShoppingList.cs
    public class ShoppingList
    {
        public List<Ingredient> Items { get; set; } = new List<Ingredient>();

        public void AddRecipeIngredients(Recipe recipe)
        {
            Items.AddRange(recipe.Ingredients);
        }

        public void RemoveIngredient(Guid itemId)
        {
            Items.RemoveAll(i => i.Id == itemId);
        }

        public List<Ingredient> GetConsolidatedList()
        {
            // Group by name and sum the amounts
            return Items
                .GroupBy(i => i.Name)
                .Select(group => new Ingredient(
                    group.Key,
                    group.Sum(i => i.Amount),
                    group.First().Unit,
                    group.First().Category
                ))
                .ToList();
        }

        public void PrintList()
        {
            Console.WriteLine("Shopping List:");
            foreach (var item in Items)
            {
                Console.WriteLine($"  [{item.Category}] {item.Amount}{item.Unit} of {item.Name}");
            }
        }
    }
}
