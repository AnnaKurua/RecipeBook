namespace RecipeBook
{
    public enum RecipeChangeKind { Created, Updated, Deleted }

    
    /// Data object carried by every Observer notification.
    /// Tells subscribers what happened and which recipe was involved.
    
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
