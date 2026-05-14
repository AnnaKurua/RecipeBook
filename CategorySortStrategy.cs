using System.Collections.Generic;
using System.Linq;

namespace RecipeBook
{
    public class CategorySortStrategy : ISortStrategy
    {
        public List<Ingredient> Sort(List<Ingredient> items)
        {
            return items.OrderBy(i => i.Category).ToList();
        }
    }
}