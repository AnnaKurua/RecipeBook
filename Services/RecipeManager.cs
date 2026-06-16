using System;
using System.Collections.Generic;

namespace RecipeBook
{
    public class RecipeManager
    {
        private readonly List<ISubscriber> _subscribers = new();

        public void Subscribe(ISubscriber subscriber)
        {
            if (subscriber == null) throw new ArgumentNullException(nameof(subscriber));

            if (!_subscribers.Contains(subscriber))
                _subscribers.Add(subscriber);
        }

        public void Unsubscribe(ISubscriber subscriber) =>
            _subscribers.Remove(subscriber);

        public void Notify(RecipeChangedEvent evt)
        {
            if (evt == null) return;

            foreach (var subscriber in _subscribers)
                subscriber.OnRecipeChanged(evt);
        }
    }
}