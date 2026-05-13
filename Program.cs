// Program.cs — complete version with both tests

// --- Create Recipes ---
using RecipeBook;

Recipe pasta = new Recipe("Spaghetti Bolognese", "A classic Italian pasta dish");
pasta.AddIngredient(new Ingredient("Spaghetti", 200, "grams", "Pasta"));
pasta.AddIngredient(new Ingredient("Ground Beef", 300, "grams", "Meat"));
pasta.AddIngredient(new Ingredient("Tomato Sauce", 150, "ml", "Canned Goods"));

Recipe cake = new Recipe("Chocolate Cake", "Rich and moist");
cake.AddIngredient(new Ingredient("Flour", 300, "grams", "Baking"));
cake.AddIngredient(new Ingredient("Spaghetti", 100, "grams", "Pasta")); // shared ingredient!

// --- Test 1: Print and Scale Pasta ---
Console.WriteLine($"Recipe: {pasta.Title}");
Console.WriteLine("Ingredients:");
foreach (var ingredient in pasta.Ingredients)
{
    Console.WriteLine($"  - {ingredient.Amount}{ingredient.Unit} of {ingredient.Name}");
}

pasta.Scale(2);
Console.WriteLine("\nAfter scaling to 2 servings:");
foreach (var ingredient in pasta.Ingredients)
{
    Console.WriteLine($"  - {ingredient.Amount}{ingredient.Unit} of {ingredient.Name}");
}

// --- Test 2: Shopping List ---
ShoppingList list = new ShoppingList();
list.AddRecipeIngredients(pasta);
list.AddRecipeIngredients(cake);

Console.WriteLine("\n--- Raw List ---");
list.PrintList();

Console.WriteLine("\n--- Consolidated List (duplicates merged) ---");
foreach (var item in list.GetConsolidatedList())
{
    Console.WriteLine($"  [{item.Category}] {item.Amount}{item.Unit} of {item.Name}");
}

Console.ReadLine();