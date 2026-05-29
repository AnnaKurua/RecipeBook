using System;

namespace RecipeBook
{
    /// <summary>
    /// Seeds two sample recipes on first run (when the repository is empty).
    /// Uses RecipeBuilder to demonstrate the Builder pattern in action.
    /// </summary>
    internal static class SampleDataSeeder
    {
        public static void SeedIfEmpty(IRecipeRepo recipeRepo)
        {
            if (recipeRepo == null) return;
            if (recipeRepo.GetAll().Count > 0) return;

            // FIX: Instantiating Ingredients explicitly to circumvent missing method signatures
            var pasta = new RecipeBuilder()
                .WithTitle("Spaghetti Bolognese")
                .WithDescription("A classic Italian pasta dish")
                .WithServings(2)
                .AddIngredient(IngredientFactory.Create("Spaghetti", 200, "grams", "Pasta"))
                .AddIngredient(IngredientFactory.Create("Ground Beef", 300, "grams", "Meat"))
                .AddIngredient(IngredientFactory.Create("Tomato Sauce", 400, "ml", "Canned Goods"))
                .AddStep("Boil pasta in salted water until al dente.")
                .AddStep("Brown the beef in a pan, add tomato sauce and simmer 20 min.")
                .AddStep("Combine and serve.")
                .Build();

            var cake = new RecipeBuilder()
                .WithTitle("Chocolate Cake")
                .WithDescription("Rich and moist dessert")
                .WithServings(8)
                .AddIngredient(IngredientFactory.Create("Flour", 300, "grams", "Baking"))
                .AddIngredient(IngredientFactory.Create("Sugar", 200, "grams", "Baking"))
                .AddIngredient(IngredientFactory.Create("Cocoa Powder", 75, "grams", "Baking"))
                .AddIngredient(IngredientFactory.Create("Eggs", 3, "pcs", "Dairy"))
                .AddStep("Mix all dry ingredients.")
                .AddStep("Beat eggs and combine with dry mix.")
                .AddStep("Bake at 180 °C for 35 minutes.")
                .Build();

            recipeRepo.Save(pasta);
            recipeRepo.Save(cake);
        }
    }
}