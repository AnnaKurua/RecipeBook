namespace RecipeBook
{
    /// <summary>
    /// Observer pattern — subject that notifies subscribers when recipes change.
    /// </summary>
    public class RecipeManager
    {
        private readonly List<ISubscriber> subscribers = new();

        public void Subscribe(ISubscriber subscriber)
        {
            if (!subscribers.Contains(subscriber))
            {
                subscribers.Add(subscriber);
            }
        }

        public void Unsubscribe(ISubscriber subscriber)
        {
            subscribers.Remove(subscriber);
        }

        public void Notify(object data)
        {
            foreach (var subscriber in subscribers)
            {
                subscriber.Update(data);
            }
        }
    }
}
