namespace RecipeBook
{
    public enum RecipeChangeKind { Created, Updated, Deleted }

    /// <summary>
    /// Data object carried by every Observer notification.
    /// Tells subscribers what happened and which recipe was involved.
    /// </summary>
    public class RecipeChangedEvent
    {
        public RecipeChangeKind Kind   { get; }
        public Recipe           Recipe { get; }

        public RecipeChangedEvent(RecipeChangeKind kind, Recipe recipe)
        {
            Kind   = kind;
            Recipe = recipe;
        }

        public override string ToString() =>
            $"[{Kind}] Recipe '{Recipe.Title}'";
    }
}
