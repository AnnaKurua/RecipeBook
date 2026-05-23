namespace RecipeBook
{
    /// <summary>
    /// Singleton — single configuration point for data file paths.
    /// </summary>
    public sealed class AppSettings
    {
        private static AppSettings? instance;

        private AppSettings()
        {
            DataDirectory = Path.Combine(AppContext.BaseDirectory, "data");
            Directory.CreateDirectory(DataDirectory);
        }

        public static AppSettings Instance => instance ??= new AppSettings();

        public string DataDirectory { get; }

        public string UsersFile => Path.Combine(DataDirectory, "users.json");

        public string RecipesFile => Path.Combine(DataDirectory, "recipes.json");
    }
}
