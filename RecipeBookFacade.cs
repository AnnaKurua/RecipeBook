using System;
using System.Collections.Generic;

namespace RecipeBook
{
    /// <summary>
    /// PATTERN: Facade
    /// Single entry point for the whole application.
    /// Hides UserManagement, RecipeService, and RecipeManager behind
    /// one Run() call and a set of private workflow methods.
    /// </summary>
    public class RecipeBookFacade
    {
        private readonly UserManagement _userManagement;
        private readonly RecipeService _recipeService;
        private readonly RecipeManager _recipeManager;
        private User? _currentUser;

        // PATTERN: Command — history stack enables Undo
        // FIX 1: Added a Stack to track executed commands
        private readonly Stack<IShoppingListCommand> _commandHistory = new();

        public RecipeBookFacade(
            UserManagement userManagement,
            RecipeService recipeService,
            RecipeManager recipeManager)
        {
            _userManagement = userManagement ?? throw new ArgumentNullException(nameof(userManagement));
            _recipeService = recipeService ?? throw new ArgumentNullException(nameof(recipeService));
            _recipeManager = recipeManager ?? throw new ArgumentNullException(nameof(recipeManager));
        }

        // ── Entry point ───────────────────────────────────────────────────────

        public void Run()
        {
            PrintHeader();
            if (!EnsureUser()) return;

            bool running = true;
            while (running)
            {
                Console.WriteLine();
                Console.WriteLine($"Logged in as: {_currentUser!.Name} ({_currentUser.Email})");
                Console.WriteLine("=== MAIN MENU ===");
                Console.WriteLine("1. Browse / add recipes");
                Console.WriteLine("2. View my saved recipes");
                Console.WriteLine("3. Manage shopping list");
                Console.WriteLine("4. Update email");
                Console.WriteLine("5. Switch user");
                Console.WriteLine("6. Undo last action");
                Console.WriteLine("0. Exit");
                Console.Write("Choice: ");

                switch (Console.ReadLine()?.Trim())
                {
                    case "1": RecipeWorkflow(); break;
                    case "2": ViewSavedRecipes(); break;
                    case "3": ShoppingListWorkflow(); break;
                    case "4": UpdateEmailWorkflow(); break;
                    case "5": EnsureUser(); break;
                    // FIX 2: Case "6" was completely missing — added undo logic
                    case "6": UndoLastAction(); break;
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

        // ── Undo ──────────────────────────────────────────────────────────────

        // FIX 3: New method that pops and reverses the last command
        private void UndoLastAction()
        {
            if (_commandHistory.Count == 0)
            {
                Console.WriteLine("Nothing to undo.");
                return;
            }

            IShoppingListCommand last = _commandHistory.Pop();
            last.Undo();
            _userManagement.SaveUserState(_currentUser!);
            Console.WriteLine($"Undone: {last.Description}");
        }

        // ── User login / registration ─────────────────────────────────────────

        private bool EnsureUser()
        {
            // Unsubscribe the previous user's shopping list before switching
            if (_currentUser != null)
                _recipeManager.Unsubscribe(_currentUser.UserShopList);

            // FIX 4: Clear history when switching users — old commands belong to old user
            _commandHistory.Clear();

            Console.WriteLine();
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
                    _currentUser = _userManagement.RegisterUser(name, email);
                    _recipeManager.Subscribe(_currentUser.UserShopList);
                    Console.WriteLine($"Registered: {_currentUser.Name}");
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
                _currentUser = _userManagement.LoginByEmail(email);

                if (_currentUser == null)
                {
                    Console.WriteLine("User not found. Please register first.");
                    return false;
                }

                _recipeManager.Subscribe(_currentUser.UserShopList);
                Console.WriteLine($"Welcome back, {_currentUser.Name}!");
                return true;
            }

            Console.WriteLine("Invalid choice.");
            return false;
        }

        // ── Recipe workflow ───────────────────────────────────────────────────

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
                    _recipeService.CreateRecipe(recipe);
                    _currentUser!.AddSavedRecipe(recipe);
                    _userManagement.SaveUserState(_currentUser);
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

            if (selected == null) return;

            DisplayRecipe(selected);

            Console.Write("Generate shopping list from this recipe? (y/n): ");
            if (Console.ReadLine()?.Trim().ToLowerInvariant() == "y")
            {
                _currentUser!.UserShopList.AddRecipeIngredients(selected);
                _userManagement.SaveUserState(_currentUser);
                Console.WriteLine("Ingredients added to your shopping list.");
                ShoppingListWorkflow();
            }
        }

        private Recipe? SelectExistingRecipe()
        {
            List<Recipe> recipes = _recipeService.BrowseAllRecipes();
            if (recipes.Count == 0)
            {
                Console.WriteLine("No recipes in the repository yet.");
                return null;
            }

            Console.WriteLine("Available recipes:");
            for (int i = 0; i < recipes.Count; i++)
                Console.WriteLine($"  {i + 1}. {recipes[i].Title} ({recipes[i].Servings} servings)");

            Console.Write("Select number: ");
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > recipes.Count)
            {
                Console.WriteLine("Invalid selection.");
                return null;
            }

            Recipe chosen = recipes[index - 1];
            _currentUser!.AddSavedRecipe(chosen);
            _userManagement.SaveUserState(_currentUser);

            Console.Write($"Scale recipe? Enter target servings (current: {chosen.Servings}, or 0 to skip): ");
            if (int.TryParse(Console.ReadLine(), out int servings) && servings > 0)
            {
                Recipe? scaled = _recipeService.GetScaledRecipe(chosen.Id, servings);
                return scaled ?? chosen;
            }

            return chosen;
        }

        // ── View saved recipes ────────────────────────────────────────────────

        private void ViewSavedRecipes()
        {
            var saved = _currentUser!.GetSavedRecipes();
            if (saved.Count == 0) { Console.WriteLine("No saved recipes yet."); return; }
            foreach (var recipe in saved) DisplayRecipe(recipe);
        }

        // ── Shopping list workflow ────────────────────────────────────────────

        private void ShoppingListWorkflow()
        {
            ShoppingList list = _currentUser!.UserShopList;

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
                        // FIX 5: Pass _commandHistory so the command gets recorded
                        RunRemoveItemCommand(list, _commandHistory);
                        break;
                    case "4":
                        // FIX 5: Pass _commandHistory so the command gets recorded
                        RunAddItemCommand(list, _commandHistory);
                        break;
                    case "5":
                        list.Clear();
                        // Clear history too — nothing left to undo after a full clear
                        _commandHistory.Clear();
                        Console.WriteLine("Shopping list cleared.");
                        break;
                    case "0":
                        viewing = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                _userManagement.SaveUserState(_currentUser);
            }
        }

        // FIX 6: Both helpers now accept the history stack and push to it after Execute()
        private static void RunRemoveItemCommand(ShoppingList list, Stack<IShoppingListCommand> history)
        {
            var display = list.GetConsolidatedList();
            if (display.Count == 0) { Console.WriteLine("  (empty)"); return; }

            for (int i = 0; i < display.Count; i++)
            {
                var item = display[i];
                Console.WriteLine($"  {i + 1}. [{item.Category}] {item.Amount:G} {item.Unit} {item.Name}");
            }

            Console.Write("Enter item number to mark as bought: ");
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > display.Count)
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            Ingredient match = display[index - 1];
            IShoppingListCommand cmd = new RemoveIngredientCommand(list, match.Name, match.Unit);
            cmd.Execute();
            history.Push(cmd); // record for undo
            Console.WriteLine(cmd.Description + " — done.");
        }

        private static void RunAddItemCommand(ShoppingList list, Stack<IShoppingListCommand> history)
        {
            Ingredient? ingredient = IngredientFactory.CreateFromConsole();
            if (ingredient == null) return;

            IShoppingListCommand cmd = new AddIngredientCommand(list, ingredient);
            cmd.Execute();
            history.Push(cmd); // record for undo
            Console.WriteLine(cmd.Description + " — done.");
        }

        // ── Email update ──────────────────────────────────────────────────────

        private void UpdateEmailWorkflow()
        {
            Console.Write("New email: ");
            string email = Console.ReadLine() ?? string.Empty;
            try
            {
                _userManagement.UpdateEmail(_currentUser!.Id, email);
                _currentUser.Email = email;
                Console.WriteLine("Email updated.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update failed: {ex.Message}");
            }
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private static void DisplayRecipe(Recipe recipe)
        {
            Console.WriteLine();
            Console.WriteLine($"--- {recipe.Title} ({recipe.Servings} servings) ---");
            Console.WriteLine(recipe.Description);
            Console.WriteLine("Ingredients:");
            foreach (var ing in recipe.Ingredients)
                Console.WriteLine($"  - {ing.Amount:G} {ing.Unit} {ing.Name} [{ing.Category}]");

            if (recipe.Steps.Count > 0)
            {
                Console.WriteLine("Steps:");
                for (int i = 0; i < recipe.Steps.Count; i++)
                    Console.WriteLine($"  {i + 1}. {recipe.Steps[i]}");
            }
        }

        private static void PrintHeader()
        {
            Console.WriteLine("========================================");
            Console.WriteLine("    Recipe Book & Shopping List");
            Console.WriteLine("========================================");
        }
    }
}