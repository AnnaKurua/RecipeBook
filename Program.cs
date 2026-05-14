using System;
using RecipeBook;

Recipe pasta = new Recipe("Spaghetti Bolognese", "A classic Italian pasta dish");
pasta.AddIngredient(new Ingredient("Spaghetti", 200, "grams", "Pasta"));
pasta.AddIngredient(new Ingredient("Ground Beef", 300, "grams", "Meat"));
pasta.AddIngredient(new Ingredient("Tomato Sauce", 150, "ml", "Canned Goods"));

Recipe cake = new Recipe("Chocolate Cake", "Rich and moist");
cake.AddIngredient(new Ingredient("Flour", 300, "grams", "Baking"));
cake.AddIngredient(new Ingredient("Sugar", 150, "grams", "Baking"));
cake.AddIngredient(new Ingredient("Spaghetti", 100, "grams", "Pasta"));

// Repository Pattern test
IRecipeRepo repo = new JsonRecipeRepo();
RecipeService service = new RecipeService(repo);

service.AddRecipe(pasta);
service.AddRecipe(cake);

Console.WriteLine("=== SAVED RECIPES ===");
foreach (var recipe in service.GetAll())
{
    Console.WriteLine($"Recipe: {recipe.Title}");
}

// Recipe test
Console.WriteLine("\n=== PASTA INGREDIENTS BEFORE SCALING ===");
foreach (var ingredient in pasta.Ingredients)
{
    Console.WriteLine($"- {ingredient.Amount} {ingredient.Unit} of {ingredient.Name}");
}

pasta.Scale(2);

Console.WriteLine("\n=== PASTA INGREDIENTS AFTER SCALING ===");
foreach (var ingredient in pasta.Ingredients)
{
    Console.WriteLine($"- {ingredient.Amount} {ingredient.Unit} of {ingredient.Name}");
}

// Shopping List test
ShoppingList list = new ShoppingList();
list.AddRecipeIngredients(pasta);
list.AddRecipeIngredients(cake);

Console.WriteLine("\n=== RAW SHOPPING LIST ===");
list.PrintList();

Console.WriteLine("\n=== CONSOLIDATED SHOPPING LIST ===");
foreach (var item in list.GetConsolidatedList())
{
    Console.WriteLine($"[{item.Category}] {item.Amount} {item.Unit} of {item.Name}");
}

// Strategy Pattern test
Console.WriteLine("\n=== SORTED BY NAME ===");
list.SetSortStrategy(new AlphabeticalSortStrategy());

foreach (var item in list.GetSortedList())
{
    Console.WriteLine($"[{item.Category}] {item.Amount} {item.Unit} of {item.Name}");
}

Console.WriteLine("\n=== SORTED BY CATEGORY ===");
list.SetSortStrategy(new CategorySortStrategy());

foreach (var item in list.GetSortedList())
{
    Console.WriteLine($"[{item.Category}] {item.Amount} {item.Unit} of {item.Name}");
}

// Observer Pattern test
Console.WriteLine("\n=== OBSERVER TEST ===");
RecipeManager manager = new RecipeManager();

manager.Subscriber(list);
manager.Notify("Recipe list changed");

// User test
Console.WriteLine("\n=== USER TEST ===");
User user = new User("Giorgi");

Console.WriteLine($"User: {user.Name}");
Console.WriteLine($"Saved recipes count: {user.GetSavedRecipes().Count}");

Console.WriteLine("\nProgram finished.");
Console.ReadLine();