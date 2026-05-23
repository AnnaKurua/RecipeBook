namespace RecipeBook
{
    internal static class SampleDataSeeder
    {
        public static void SeedIfEmpty(IRecipeRepo recipeRepo)
        {
            if (recipeRepo.GetAll().Count > 0)
            {
                return;
            }

            var pasta = new RecipeBuilder()
                .WithTitle("Spaghetti Bolognese")
                .WithDescription("A classic Italian pasta dish")
                .WithServings(2)
                .AddIngredient(IngredientFactory.Create("Spaghetti", 200, "grams", "Pasta"))
                .AddIngredient(IngredientFactory.Create("Ground Beef", 300, "grams", "Meat"))
                .AddIngredient(IngredientFactory.Create("Tomato Sauce", 400, "ml", "Canned Goods"))
                .AddStep("Boil pasta until al dente.")
                .AddStep("Brown beef and simmer with sauce.")
                .Build();

            var cake = new RecipeBuilder()
                .WithTitle("Chocolate Cake")
                .WithDescription("Rich and moist dessert")
                .WithServings(8)
                .AddIngredient(IngredientFactory.Create("Flour", 300, "grams", "Baking"))
                .AddIngredient(IngredientFactory.Create("Sugar", 200, "grams", "Baking"))
                .AddIngredient(IngredientFactory.Create("Cocoa Powder", 75, "grams", "Baking"))
                .AddStep("Mix dry ingredients.")
                .AddStep("Bake at 180°C for 35 minutes.")
                .Build();

            recipeRepo.Save(pasta);
            recipeRepo.Save(cake);
        }
    }
}
