namespace RecipeBook
{
    /// <summary>
    /// Decorator — adds validation before delegating save to the inner repository.
    /// </summary>
    public class ValidatingRecipeRepository : IRecipeRepo
    {
        private readonly IRecipeRepo inner;
        private readonly IValidationHandler validationChain;

        public ValidatingRecipeRepository(IRecipeRepo inner, IValidationHandler? validationChain = null)
        {
            this.inner = inner;
            this.validationChain = validationChain ?? RecipeValidationChain.Build();
        }

        public List<Recipe> GetAll() => inner.GetAll();

        public Recipe? GetById(Guid id) => inner.GetById(id);

        public void Save(Recipe recipe)
        {
            ValidationResult result = validationChain.Validate(recipe);
            if (!result.IsValid)
            {
                throw new InvalidOperationException(result.Message);
            }

            inner.Save(recipe);
        }

        public void Delete(Guid id) => inner.Delete(id);
    }
}
