using System.Collections.Generic;

namespace RecipeBook
{
    public interface ISortStrategy
    {
        List<Ingredient> Sort(List<Ingredient> items);
    }
}