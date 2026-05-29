using System;
using System.IO;

namespace RecipeBook
{
    /// <summary>
    /// PATTERN: Singleton
    /// One shared configuration object for the whole application.
    /// Centralises file paths so they never get out of sync.
    /// </summary>
    public sealed class AppSettings
    {
        private static AppSettings? _instance;
        private static readonly object _padlock = new object(); // FIX: Safety thread lock

        private AppSettings()
        {
            DataDirectory = Path.Combine(AppContext.BaseDirectory, "data");
            Directory.CreateDirectory(DataDirectory);
        }

        public static AppSettings Instance
        {
            get
            {
                // FIX: Implemented double-check thread-safe instantiation locking
                if (_instance == null)
                {
                    lock (_padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new AppSettings();
                        }
                    }
                }
                return _instance;
            }
        }

        public string DataDirectory { get; }
        public string UsersFile => Path.Combine(DataDirectory, "users.json");
        public string RecipesFile => Path.Combine(DataDirectory, "recipes.json");
    }
}