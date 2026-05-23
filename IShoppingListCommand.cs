namespace RecipeBook
{
    /// <summary>
    /// Command — encapsulates shopping-list modifications.
    /// </summary>
    public interface IShoppingListCommand
    {
        void Execute();
        string Description { get; }
    }
}
