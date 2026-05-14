using System.Collections.Generic;

namespace RecipeBook
{
    public class RecipeManager
    {
        private List<ISubscriber> subscribers = new List<ISubscriber>();

        public void Subscriber(ISubscriber subscriber)
        {
            subscribers.Add(subscriber);
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