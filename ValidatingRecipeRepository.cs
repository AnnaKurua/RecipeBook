using System;
using System.Collections.Generic;

namespace RecipeBook
{
    /// <summary>
    /// PATTERN: Decorator
    /// Wraps any IRecipeRepo and intercepts Save() to run the validation
    /// chain before delegating. The inner repo stays unchanged.
    /// </summary>
    public class ValidatingRecipeRepository : IRecipeRepo
    {
        private readonly IRecipeRepo _inner;
        private readonly IValidationHandler _validationChain;

        public ValidatingRecipeRepository(IRecipeRepo inner, IValidationHandler? chain = null)
        {
            // FIX: Added standard defensive guard checks
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            _validationChain = chain ?? RecipeValidationChain.Build();
        }

        public List<Recipe> GetAll() => _inner.GetAll();
        public Recipe? GetById(Guid id) => _inner.GetById(id);
        public void Delete(Guid id) => _inner.Delete(id);

        public void Save(Recipe recipe)
        {
            // FIX: Guard check to catch empty inputs instantly
            if (recipe == null) throw new ArgumentNullException(nameof(recipe));

            ValidationResult result = _validationChain.Validate(recipe);
            if (!result.IsValid)
                throw new InvalidOperationException(result.Message);

            _inner.Save(recipe);
        }
    }
}