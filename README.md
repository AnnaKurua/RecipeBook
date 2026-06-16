# RecipeBook

RecipeBook is a C# application with a web GUI served by an ASP.NET minimal API and backed by MongoDB.

## Important
The latest working version is on this branch:

- https://github.com/AnnaKurua/RecipeBook/tree/Add-web-API-server,-GUI-launch,-and-MongoDB-backed-login-for-web-UI

## Requirements
- .NET SDK **10** (project targets `net10.0`)
- MongoDB running locally on `mongodb://localhost:27017`
- Port **5042** must be free

## How to run (Visual Studio)
1. Open `RecipeBook.slnx`
2. Build → **Rebuild Solution**
3. Select run profile **RecipeBook** (not Console)
4. Press **F5**
5. Open `http://localhost:5042` (if it doesn’t open automatically)

## How to run (CLI)
```bash
git clone --branch "Add-web-API-server,-GUI-launch,-and-MongoDB-backed-login-for-web-UI" https://github.com/AnnaKurua/RecipeBook.git
cd RecipeBook
dotnet build
dotnet run
```

Then open `http://localhost:5042`.

## Login
- Login is **email only** (same as the console version)
- First-time users should **Register** with name + email

## Notes / Troubleshooting
- If you see “address already in use”, stop any running `RecipeBook.exe` and try again (port 5042 conflict).
- Do not open `wwwroot/index.html` directly from the filesystem — run via the server on `http://localhost:5042`.

