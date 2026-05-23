namespace RecipeBook
{
    /// <summary>
    /// Builder — step-by-step construction of a complex Recipe object.
    /// </summary>
    public class RecipeBuilder
    {
        private string title = string.Empty;
        private string description = string.Empty;
        private int servings = 1;
        private readonly List<Ingredient> ingredients = new();
        private readonly List<string> steps = new();

        public RecipeBuilder WithTitle(string value)
        {
            title = value;
            return this;
        }

        public RecipeBuilder WithDescription(string value)
        {
            description = value;
            return this;
        }

        public RecipeBuilder WithServings(int value)
        {
            servings = value;
            return this;
        }

        public RecipeBuilder AddIngredient(Ingredient ingredient)
        {
            ingredients.Add(ingredient);
            return this;
        }

        public RecipeBuilder AddStep(string step)
        {
            if (!string.IsNullOrWhiteSpace(step))
            {
                steps.Add(step);
            }

            return this;
        }

        public Recipe Build()
        {
            var recipe = new Recipe(title, description, servings);
            recipe.Ingredients.AddRange(ingredients);
            recipe.Steps.AddRange(steps);
            return recipe;
        }

        public static Recipe BuildFromConsole()
        {
            var builder = new RecipeBuilder();

            Console.Write("Recipe title: ");
            builder.WithTitle(Console.ReadLine() ?? "Untitled");

            Console.Write("Description: ");
            builder.WithDescription(Console.ReadLine() ?? string.Empty);

            Console.Write("Servings (default 1): ");
            if (int.TryParse(Console.ReadLine(), out int servings) && servings > 0)
            {
                builder.WithServings(servings);
            }

            Console.WriteLine("Add ingredients (empty name to finish):");
            while (true)
            {
                Ingredient? ingredient = IngredientFactory.CreateFromConsole();
                if (ingredient == null)
                {
                    break;
                }

                builder.AddIngredient(ingredient);
            }

            Console.WriteLine("Add preparation steps (empty line to finish):");
            while (true)
            {
                Console.Write("Step: ");
                string? step = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(step))
                {
                    break;
                }

                builder.AddStep(step);
            }

            return builder.Build();
        }
    }
}
