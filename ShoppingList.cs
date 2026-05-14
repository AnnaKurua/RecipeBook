using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeBook
{
    public class ShoppingList : ISubscriber
    {
        public List<Ingredient> Items { get; set; } = new List<Ingredient>();

        private ISortStrategy sortStrategy;

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
            return Items
                .GroupBy(i => new { i.Name, i.Unit })
                .Select(group => new Ingredient(
                    group.Key.Name,
                    group.Sum(i => i.Amount),
                    group.Key.Unit,
                    group.First().Category
                ))
                .ToList();
        }

        public void SetSortStrategy(ISortStrategy strategy)
        {
            sortStrategy = strategy;
        }

        public List<Ingredient> GetSortedList()
        {
            if (sortStrategy == null)
            {
                return Items;
            }

            return sortStrategy.Sort(Items);
        }

        public void Update(object data)
        {
            Console.WriteLine($"Shopping list received update: {data}");
        }

        public void PrintList()
        {
            Console.WriteLine("Shopping List:");

            foreach (var item in Items)
            {
                Console.WriteLine($"  [{item.Category}] {item.Amount} {item.Unit} of {item.Name}");
            }
        }
    }
}