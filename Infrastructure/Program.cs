using System;
using RecipeBook;

namespace RecipeBook
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Contains("--console"))
                RunConsoleApp();
            else
                WebApiHost.Run(args);
        }

        static void RunConsoleApp()
        {
            MongoConfig.Configure();
            IRecipeRepo recipeRepo = new ValidatingRecipeRepository(new MongoRecipeRepo());
            SampleDataSeeder.SeedIfEmpty(recipeRepo);

            IUserRepo userRepo = new MongoUserRepo();
            RecipeManager recipeManager = new RecipeManager();
            RecipeService recipeService = new RecipeService(recipeRepo, recipeManager);
            UserManagement userMgmt = new UserManagement(userRepo, recipeRepo);

            var app = new RecipeBookFacade(userMgmt, recipeService, recipeManager);
            app.Run();
        }
    }
}
