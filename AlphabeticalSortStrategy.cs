using System.Collections.Generic;
using System.Linq;

namespace RecipeBook
{
    public class AlphabeticalSortStrategy : ISortStrategy
    {
        public List<Ingredient> Sort(List<Ingredient> items)
        {
            return items.OrderBy(i => i.Name).ToList();
        }
    }
}