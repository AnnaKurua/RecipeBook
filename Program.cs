using RecipeBook;

// Composition root — wire dependencies (DIP: depend on abstractions)
IRecipeRepo recipeRepo = new ValidatingRecipeRepository(new JsonRecipeRepo());
SampleDataSeeder.SeedIfEmpty(recipeRepo);
IUserRepo userRepo = new JsonUserRepo();
var recipeManager = new RecipeManager();

var recipeService = new RecipeService(recipeRepo, recipeManager);
var userManagement = new UserManagement(userRepo, recipeRepo);

var app = new RecipeBookFacade(userManagement, recipeService, recipeManager);
app.Run();