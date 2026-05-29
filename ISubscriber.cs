namespace RecipeBook
{
    /// <summary>
    /// PATTERN: Observer (subscriber side)
    /// Implement this to react to recipe-change events published by RecipeManager.
    /// </summary>
    public interface ISubscriber
    {
        void OnRecipeChanged(RecipeChangedEvent evt);
    }
}
