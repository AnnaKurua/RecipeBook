using System;
using System.Collections.Generic;

namespace RecipeBook
{
    /// <summary>
    /// PATTERN: Builder
    /// Constructs a Recipe step by step using a fluent API.
    /// Separates "how a recipe is assembled" from the Recipe class itself.
    /// </summary>
    public class RecipeBuilder
    {
        private string _title = string.Empty;
        private string _description = string.Empty;
        private string _category = string.Empty;
        private int _servings = 1;
        private readonly List<Ingredient> _ingredients = new();
        private readonly List<string> _steps = new();

        public RecipeBuilder WithTitle(string value) { _title = value ?? string.Empty; return this; }
        public RecipeBuilder WithDescription(string value) { _description = value ?? string.Empty; return this; }
        public RecipeBuilder WithCategory(string value) { _category = value ?? string.Empty; return this; }
        public RecipeBuilder WithServings(int value) { _servings = value > 0 ? value : 1; return this; }

        public RecipeBuilder AddIngredient(Ingredient ingredient)
        {
            if (ingredient != null)
            {
                _ingredients.Add(ingredient);
            }
            return this;
        }

        public RecipeBuilder AddStep(string step)
        {
            if (!string.IsNullOrWhiteSpace(step))
            {
                _steps.Add(step.Trim());
            }
            return this;
        }

        public Recipe Build()
        {
            var recipe = new Recipe(_title, _description, _servings)
            {
                Category = _category
            };

            // FIX: Using formal domain methods instead of breaking encapsulation via .AddRange
            foreach (var ingredient in _ingredients)
            {
                recipe.AddIngredient(ingredient);
            }

            foreach (var step in _steps)
            {
                recipe.AddStep(step);
            }

            return recipe;
        }

        // ── Interactive console builder ──────────────────────────────────────

        /// <summary>
        /// Guides the user through entering a recipe at the console, then calls Build().
        /// </summary>
        public static Recipe BuildFromConsole()
        {
            var builder = new RecipeBuilder();

            Console.Write("Recipe title: ");
            builder.WithTitle(Console.ReadLine() ?? "Untitled");

            Console.Write("Description: ");
            builder.WithDescription(Console.ReadLine() ?? string.Empty);

            Console.Write("Servings (default 1): ");
            if (int.TryParse(Console.ReadLine(), out int servings) && servings > 0)
                builder.WithServings(servings);

            Console.WriteLine("Add ingredients (blank name to finish):");
            while (true)
            {
                // NOTE: This relies on the IngredientFactory class. 
                // Visual Studio might mark this line with a red squiggly if we haven't brought that file in yet.
                Ingredient? ingredient = IngredientFactory.CreateFromConsole();
                if (ingredient == null) break;
                builder.AddIngredient(ingredient);
            }

            Console.WriteLine("Add preparation steps (blank line to finish):");
            while (true)
            {
                Console.Write("  Step: ");
                string? step = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(step)) break;
                builder.AddStep(step);
            }

            return builder.Build();
        }
    }
}