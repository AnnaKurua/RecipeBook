namespace RecipeBook
{
    public class Recipe
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Ingredient> Ingredients { get; set; } = new();
        public List<string> Steps { get; set; } = new();
        public int Servings { get; set; } = 1;

        public Recipe()
        {
        }

        public Recipe(string title, string description, int servings = 1)
        {
            Title = title;
            Description = description;
            Servings = servings > 0 ? servings : 1;
        }

        public void AddIngredient(Ingredient ingredient)
        {
            Ingredients.Add(ingredient);
        }

        public void RemoveIngredient(Guid ingredientId)
        {
            Ingredients.RemoveAll(i => i.Id == ingredientId);
        }

        public void Scale(int targetServings)
        {
            if (targetServings <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(targetServings), "Servings must be greater than zero.");
            }

            int baseServings = Servings > 0 ? Servings : 1;
            double factor = (double)targetServings / baseServings;

            foreach (var ingredient in Ingredients)
            {
                ingredient.Amount *= factor;
            }

            Servings = targetServings;
        }

        public Recipe Clone()
        {
            var copy = new Recipe(Title, Description, Servings)
            {
                Id = Id,
                Steps = new List<string>(Steps)
            };

            foreach (var ingredient in Ingredients)
            {
                copy.Ingredients.Add(ingredient.Clone());
            }

            return copy;
        }
    }
}
