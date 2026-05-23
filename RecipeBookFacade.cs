namespace RecipeBook
{
    /// <summary>
    /// Facade — single entry point that orchestrates the full system workflow.
    /// </summary>
    public class RecipeBookFacade
    {
        private readonly UserManagement userManagement;
        private readonly RecipeService recipeService;
        private readonly RecipeManager recipeManager;
        private User? currentUser;

        public RecipeBookFacade(
            UserManagement userManagement,
            RecipeService recipeService,
            RecipeManager recipeManager)
        {
            this.userManagement = userManagement;
            this.recipeService = recipeService;
            this.recipeManager = recipeManager;
        }

        public void Run()
        {
            PrintHeader();
            if (!EnsureUser())
            {
                return;
            }

            bool running = true;
            while (running)
            {
                Console.WriteLine();
                Console.WriteLine($"Logged in as: {currentUser!.Name} ({currentUser.Email})");
                Console.WriteLine("=== MAIN MENU ===");
                Console.WriteLine("1. Add new recipe or choose existing");
                Console.WriteLine("2. View my saved recipes");
                Console.WriteLine("3. Manage shopping list");
                Console.WriteLine("4. Update email");
                Console.WriteLine("5. Switch user");
                Console.WriteLine("0. Exit (Cook!)");
                Console.Write("Choice: ");

                switch (Console.ReadLine()?.Trim())
                {
                    case "1":
                        RecipeWorkflow();
                        break;
                    case "2":
                        ViewSavedRecipes();
                        break;
                    case "3":
                        ShoppingListWorkflow();
                        break;
                    case "4":
                        UpdateEmailWorkflow();
                        break;
                    case "5":
                        EnsureUser();
                        break;
                    case "0":
                        Console.WriteLine("Happy cooking!");
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        private bool EnsureUser()
        {
            Console.WriteLine("=== USER ===");
            Console.WriteLine("1. Register new user");
            Console.WriteLine("2. Login with email");
            Console.Write("Choice: ");

            string? choice = Console.ReadLine()?.Trim();
            if (choice == "1")
            {
                Console.Write("Name: ");
                string name = Console.ReadLine() ?? string.Empty;
                Console.Write("Email: ");
                string email = Console.ReadLine() ?? string.Empty;

                try
                {
                    currentUser = userManagement.RegisterUser(name, email);
                    recipeManager.Subscribe(currentUser.UserShopList);
                    Console.WriteLine($"Registered: {currentUser.Name}");
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Registration failed: {ex.Message}");
                    return false;
                }
            }

            if (choice == "2")
            {
                Console.Write("Email: ");
                string email = Console.ReadLine() ?? string.Empty;
                currentUser = userManagement.LoginByEmail(email);
                if (currentUser == null)
                {
                    Console.WriteLine("User not found. Please register first.");
                    return false;
                }

                recipeManager.Subscribe(currentUser.UserShopList);
                Console.WriteLine($"Welcome back, {currentUser.Name}!");
                return true;
            }

            Console.WriteLine("Invalid choice.");
            return false;
        }

        private void RecipeWorkflow()
        {
            Console.WriteLine();
            Console.WriteLine("=== RECIPES ===");
            Console.WriteLine("1. Add new recipe");
            Console.WriteLine("2. Choose existing recipe");
            Console.Write("Choice: ");

            Recipe? selected = null;

            if (Console.ReadLine()?.Trim() == "1")
            {
                try
                {
                    Recipe recipe = RecipeBuilder.BuildFromConsole();
                    recipeService.CreateRecipe(recipe);
                    currentUser!.AddSavedRecipe(recipe);
                    selected = recipe;
                    Console.WriteLine($"Recipe '{recipe.Title}' saved.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not save recipe: {ex.Message}");
                    return;
                }
            }
            else
            {
                selected = SelectExistingRecipe();
            }

            if (selected == null)
            {
                return;
            }

            DisplayRecipe(selected);
            userManagement.SaveUserState(currentUser!);

            Console.Write("Generate shopping list from this recipe? (y/n): ");
            if (Console.ReadLine()?.Trim().ToLowerInvariant() == "y")
            {
                currentUser!.UserShopList.AddRecipeIngredients(selected);
                userManagement.SaveUserState(currentUser);
                Console.WriteLine("Ingredients added to your shopping list.");
                ShoppingListWorkflow();
            }
            else
            {
                Console.WriteLine("Skipping shopping list. Ready to cook when you are!");
            }
        }

        private Recipe? SelectExistingRecipe()
        {
            List<Recipe> recipes = recipeService.BrowseAllRecipes();
            if (recipes.Count == 0)
            {
                Console.WriteLine("No recipes in the repository yet.");
                return null;
            }

            Console.WriteLine("Available recipes:");
            for (int i = 0; i < recipes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {recipes[i].Title} ({recipes[i].Servings} servings)");
            }

            Console.Write("Select number: ");
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > recipes.Count)
            {
                Console.WriteLine("Invalid selection.");
                return null;
            }

            Recipe chosen = recipes[index - 1];
            currentUser!.AddSavedRecipe(chosen);

            Console.Write($"Scale recipe? Enter target servings (current {chosen.Servings}, or 0 to skip): ");
            if (int.TryParse(Console.ReadLine(), out int servings) && servings > 0)
            {
                Recipe? scaled = recipeService.GetScaledRecipe(chosen.Id, servings);
                return scaled ?? chosen;
            }

            return chosen;
        }

        private static void DisplayRecipe(Recipe recipe)
        {
            Console.WriteLine();
            Console.WriteLine($"--- {recipe.Title} ---");
            Console.WriteLine(recipe.Description);
            Console.WriteLine($"Servings: {recipe.Servings}");
            Console.WriteLine("Ingredients:");
            foreach (var ing in recipe.Ingredients)
            {
                Console.WriteLine($"  - {ing.Amount} {ing.Unit} {ing.Name} [{ing.Category}]");
            }

            if (recipe.Steps.Count > 0)
            {
                Console.WriteLine("Steps:");
                for (int i = 0; i < recipe.Steps.Count; i++)
                {
                    Console.WriteLine($"  {i + 1}. {recipe.Steps[i]}");
                }
            }
        }

        private void ViewSavedRecipes()
        {
            var saved = currentUser!.GetSavedRecipes();
            if (saved.Count == 0)
            {
                Console.WriteLine("No saved recipes yet.");
                return;
            }

            foreach (var recipe in saved)
            {
                DisplayRecipe(recipe);
            }
        }

        private void ShoppingListWorkflow()
        {
            ShoppingList list = currentUser!.UserShopList;

            bool viewing = true;
            while (viewing)
            {
                Console.WriteLine();
                Console.WriteLine("=== SHOPPING LIST ===");
                list.Display(consolidated: true, sorted: false);
                Console.WriteLine();
                Console.WriteLine("1. Sort by name");
                Console.WriteLine("2. Sort by category");
                Console.WriteLine("3. Remove item (bought)");
                Console.WriteLine("4. Add item manually");
                Console.WriteLine("5. Clear list");
                Console.WriteLine("0. Back");
                Console.Write("Choice: ");

                switch (Console.ReadLine()?.Trim())
                {
                    case "1":
                        list.SetSortStrategy(new AlphabeticalSortStrategy());
                        Console.WriteLine("Sorted by name:");
                        list.Display(consolidated: true, sorted: true);
                        break;
                    case "2":
                        list.SetSortStrategy(new CategorySortStrategy());
                        Console.WriteLine("Sorted by category:");
                        list.Display(consolidated: true, sorted: true);
                        break;
                    case "3":
                        RemoveItemCommand(list);
                        break;
                    case "4":
                        AddItemCommand(list);
                        break;
                    case "5":
                        list.Clear();
                        Console.WriteLine("Shopping list cleared.");
                        break;
                    case "0":
                        viewing = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                userManagement.SaveUserState(currentUser);
            }
        }

        private static void RemoveItemCommand(ShoppingList list)
        {
            var display = list.GetConsolidatedList();
            if (display.Count == 0)
            {
                return;
            }

            for (int i = 0; i < display.Count; i++)
            {
                var item = display[i];
                Console.WriteLine($"  {i + 1}. [{item.Category}] {item.Amount} {item.Unit} {item.Name}");
            }

            Console.Write("Enter item number to remove (bought): ");
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > display.Count)
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            Ingredient match = display[index - 1];
            list.RemoveMatching(match.Name, match.Unit);
            Console.WriteLine($"Removed {match.Name} from shopping list.");
        }

        private static void AddItemCommand(ShoppingList list)
        {
            Ingredient? ingredient = IngredientFactory.CreateFromConsole();
            if (ingredient == null)
            {
                return;
            }

            var command = new AddIngredientCommand(list, ingredient);
            command.Execute();
            Console.WriteLine(command.Description + " — done.");
        }

        private void UpdateEmailWorkflow()
        {
            Console.Write("New email: ");
            string email = Console.ReadLine() ?? string.Empty;
            try
            {
                userManagement.UpdateEmail(currentUser!.Id, email);
                currentUser.Email = email;
                Console.WriteLine("Email updated.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update failed: {ex.Message}");
            }
        }

        private static void PrintHeader()
        {
            Console.WriteLine("========================================");
            Console.WriteLine("   Recipe Book & Shopping List Backend");
            Console.WriteLine("========================================");
        }
    }
}
