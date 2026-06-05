using System;
using RecipeBook;

namespace RecipeBook
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // ── Composition root ────────────────────────────────────────────────────────
            // This is the only place where concrete classes are instantiated.
            // Everything else depends on abstractions (interfaces), following DIP.
            MongoConfig.Configure(); // must run before any MongoDB operation
            // PATTERN: Decorator — ValidatingRecipeRepository wraps JsonRecipeRepo
            IRecipeRepo recipeRepo = new ValidatingRecipeRepository(new MongoRecipeRepo());

            // Seed sample data on the first run if file is fresh
            SampleDataSeeder.SeedIfEmpty(recipeRepo);

            IUserRepo userRepo = new MongoUserRepo();
            RecipeManager recipeManager = new RecipeManager();   // Observer subject
            RecipeService recipeService = new RecipeService(recipeRepo, recipeManager);
            UserManagement userMgmt = new UserManagement(userRepo, recipeRepo);

            // PATTERN: Facade — single Run() drives the whole application
            var app = new RecipeBookFacade(userMgmt, recipeService, recipeManager);
            app.Run();
        }
    }
}