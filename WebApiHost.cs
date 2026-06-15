using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace RecipeBook
{
    public static class WebApiHost
    {
        private const string Url = "http://localhost:5042";
        private const int Port = 5042;

        public static void Run(string[] args)
        {
            if (IsPortInUse(Port))
            {
                Console.WriteLine("RecipeBook is already running. Opening the web UI...");
                OpenBrowser();
                return;
            }

            MongoConfig.Configure();

            IRecipeRepo recipeRepo = new ValidatingRecipeRepository(new MongoRecipeRepo());
            SampleDataSeeder.SeedIfEmpty(recipeRepo);

            IUserRepo userRepo = new MongoUserRepo();
            UserManagement userMgmt = new UserManagement(userRepo, recipeRepo);
            RecipeManager recipeManager = new RecipeManager();
            RecipeService recipeService = new RecipeService(recipeRepo, recipeManager);

            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.UseUrls(Url);

            var app = builder.Build();

            app.Lifetime.ApplicationStarted.Register(OpenBrowser);

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.MapPost("/auth/register", (RegisterRequest req) =>
            {
                try
                {
                    var user = userMgmt.RegisterUser(req.Name, req.Email);
                    recipeManager.Subscribe(user.UserShopList);
                    return Results.Ok(UserDto.From(user));
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            });

            app.MapPost("/auth/login", (LoginRequest req) =>
            {
                var user = userMgmt.LoginByEmail(req.Email.Trim());
                if (user == null)
                    return Results.Unauthorized();

                recipeManager.Subscribe(user.UserShopList);
                return Results.Ok(UserDto.From(user));
            });

            app.MapGet("/recipes", () => recipeService.BrowseAllRecipes());

            app.MapPost("/recipes", (RecipeCreateDto dto) =>
            {
                var recipeBuilder = new RecipeBuilder()
                    .WithTitle(dto.Title)
                    .WithDescription(dto.Description)
                    .WithCategory(dto.Category)
                    .WithServings(dto.Servings);

                foreach (var ing in dto.Ingredients)
                {
                    recipeBuilder.AddIngredient(new Ingredient(
                        ing.Name,
                        ing.Amount,
                        ing.Unit ?? string.Empty,
                        ing.Category ?? "General"));
                }

                foreach (var step in dto.Steps)
                    recipeBuilder.AddStep(step);

                var recipe = recipeBuilder.Build();
                recipeService.CreateRecipe(recipe);
                return Results.Created($"/recipes/{recipe.Id}", recipe);
            });

            app.MapDelete("/recipes/{id:guid}", (Guid id) =>
            {
                recipeService.DeleteRecipe(id);
                return Results.NoContent();
            });

            app.MapGet("/shopping", (HttpRequest req) =>
            {
                var user = GetUser(req, userRepo);
                if (user == null) return Results.Unauthorized();
                return Results.Ok(user.UserShopList.Items);
            });

            app.MapPost("/shopping/add-recipe/{id:guid}", (Guid id, HttpRequest req) =>
            {
                var user = GetUser(req, userRepo);
                if (user == null) return Results.Unauthorized();

                var recipe = recipeService.GetRecipeDetails(id);
                if (recipe == null)
                    return Results.NotFound();

                user.UserShopList.AddRecipeIngredients(recipe);
                userMgmt.SaveUserState(user);
                return Results.Ok();
            });

            app.MapDelete("/shopping/remove", (string name, string unit, HttpRequest req) =>
            {
                var user = GetUser(req, userRepo);
                if (user == null) return Results.Unauthorized();

                user.UserShopList.RemoveMatching(name, unit);
                userMgmt.SaveUserState(user);
                return Results.NoContent();
            });

            app.MapDelete("/shopping/clear", (HttpRequest req) =>
            {
                var user = GetUser(req, userRepo);
                if (user == null) return Results.Unauthorized();

                user.UserShopList.Clear();
                userMgmt.SaveUserState(user);
                return Results.NoContent();
            });

            app.MapPost("/shopping/add-item", (IngredientDto dto, HttpRequest req) =>
            {
                var user = GetUser(req, userRepo);
                if (user == null) return Results.Unauthorized();

                user.UserShopList.AddItem(new Ingredient(
                    dto.Name,
                    dto.Amount,
                    dto.Unit ?? string.Empty,
                    dto.Category ?? "General"));
                userMgmt.SaveUserState(user);
                return Results.Ok();
            });

            Console.WriteLine($"RecipeBook web UI running at {Url}");
            app.Run();
        }

        private static void OpenBrowser()
        {
            try
            {
                Process.Start(new ProcessStartInfo(Url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not open browser automatically. Open {Url} manually. ({ex.Message})");
            }
        }

        private static bool IsPortInUse(int port)
        {
            try
            {
                using var listener = new TcpListener(IPAddress.Loopback, port);
                listener.Start();
                listener.Stop();
                return false;
            }
            catch (SocketException)
            {
                return true;
            }
        }

        private static User? GetUser(HttpRequest req, IUserRepo userRepo)
        {
            if (!req.Headers.TryGetValue("X-User-Id", out var header) ||
                !Guid.TryParse(header, out var userId))
                return null;

            return userRepo.GetUser(userId);
        }
    }

    public class RecipeCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Servings { get; set; } = 1;
        public List<IngredientDto> Ingredients { get; set; } = new();
        public List<string> Steps { get; set; } = new();
    }

    public class IngredientDto
    {
        public string Name { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string? Unit { get; set; }
        public string? Category { get; set; }
    }
}
