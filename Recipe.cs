using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace RecipeBook
{
    public class Recipe
    {
        // Properties use 'private set' so external code cannot change critical data without permission
        [BsonId]
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Servings { get; private set; } = 1;

        public List<Ingredient> Ingredients { get; private set; } = new();
        public List<string> Steps { get; private set; } = new();


        // Parameterless constructor required for JSON deserialization/database frameworks
        public Recipe() { }

        // Standard constructor for creating a brand new recipe
        public Recipe(string title, string description, int servings = 1)
        {
            Title = title;
            Description = description;
            Servings = servings > 0 ? servings : 1;
        }

        // Domain behaviors that allow controlled modifications to our lists
        public void AddIngredient(Ingredient ingredient)
        {
            if (ingredient == null) throw new ArgumentNullException(nameof(ingredient));
            Ingredients.Add(ingredient);
        }

        public void RemoveIngredient(Guid ingredientId)
        {
            Ingredients.RemoveAll(i => i.Id == ingredientId);
        }

        public void AddStep(string step)
        {
            if (!string.IsNullOrWhiteSpace(step))
            {
                Steps.Add(step);
            }
        }

        /// Scales ingredient amounts proportionally to the target number of servings.
        /// Uses the Prototype pattern to safely return a new modified instance without changing the original record.
        public Recipe ScaleToNewTarget(int targetServings)
        {
            if (targetServings <= 0)
                throw new ArgumentOutOfRangeException(nameof(targetServings), "Servings must be greater than zero.");

            // Leverage the Prototype pattern. We pass 'false' because this is a temporary calculation view, 
            // so we want to keep the original IDs intact for UI/Shopping List matching.
            Recipe clonedRecipe = this.Clone(createNewIds: false);

            double factor = (double)targetServings / (this.Servings > 0 ? this.Servings : 1);
            foreach (var ingredient in clonedRecipe.Ingredients)
            {
                ingredient.Amount *= factor;
            }

            clonedRecipe.Servings = targetServings;

            return clonedRecipe;
        }

        
        /// PATTERN: Prototype
        /// Returns a deep copy of this recipe.
        
        
        /// If true, generates brand new GUIDs (Use when a user clicks "Save Copy" or "Duplicate").
        /// If false, retains original GUIDs (Use for temporary views like scaling or printing).
       
        public Recipe Clone(bool createNewIds = false)
        {
            Guid targetRecipeId = createNewIds ? Guid.NewGuid() : this.Id;

            var copy = new Recipe(Title, Description, Servings)
            {
                Id = targetRecipeId
            };

            // Safely copy primitive strings into the new list container
            copy.Steps.AddRange(this.Steps);

            // Deep-copy each ingredient inside the recipe using its own Prototype pattern implementation
            foreach (var ingredient in this.Ingredients)
            {
                copy.Ingredients.Add(ingredient.Clone(createNewIds));
            }

            return copy;
        }
    }
}