using System;
using System.Collections.Generic;

namespace RecipeBook
{
    public class RecipeService
    {
        private readonly IRecipeRepo _repository;
        private readonly RecipeManager _recipeManager;

        public RecipeService(IRecipeRepo repository, RecipeManager recipeManager)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _recipeManager = recipeManager ?? throw new ArgumentNullException(nameof(recipeManager));
        }

        public List<Recipe> BrowseAllRecipes() => _repository.GetAll();
        public Recipe? GetRecipeDetails(Guid id) => _repository.GetById(id);

        /// <summary>
        /// PATTERN: Prototype in action.
        /// Returns a safely scaled copy — the database entry is never corrupted.
        /// </summary>
        public Recipe? GetScaledRecipe(Guid id, int targetServings)
        {
            Recipe? recipe = _repository.GetById(id);
            if (recipe == null) return null;

            // FIX: Using our updated, safe Prototype pipeline execution
            return recipe.ScaleToNewTarget(targetServings);
        }

        public void CreateRecipe(Recipe recipe)
        {
            if (recipe == null) throw new ArgumentNullException(nameof(recipe));
            _repository.Save(recipe);
            _recipeManager.Notify(new RecipeChangedEvent(RecipeChangeKind.Created, recipe));
        }

        public void UpdateRecipe(Recipe recipe)
        {
            if (recipe == null) throw new ArgumentNullException(nameof(recipe));
            _repository.Save(recipe);
            _recipeManager.Notify(new RecipeChangedEvent(RecipeChangeKind.Updated, recipe));
        }

        public void DeleteRecipe(Guid id)
        {
            Recipe? recipe = _repository.GetById(id);
            _repository.Delete(id);

            if (recipe != null)
                _recipeManager.Notify(new RecipeChangedEvent(RecipeChangeKind.Deleted, recipe));
        }
    }
}