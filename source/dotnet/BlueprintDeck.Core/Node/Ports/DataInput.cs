using System;
using System.Collections.Generic;

namespace BlueprintDeck.Node.Ports
{
    public class DataInput<T> : IInput<T>
    {
        private readonly IDisposable _subscription;
        private readonly List<Action<T>> _actions = new List<Action<T>>();

        public DataInput(IObservable<T> observable)
        {
            _subscription = observable.Subscribe(OnValue);
        }

        private void OnValue(T value)
        {
            Value = value;
            foreach (var action in _actions)
            {
                action(value);
            }
        }
        
        public void Dispose()
        {
            _subscription.Dispose();
        }

        public T? Value { get; private set; }

        public void OnData(Action<T> onValue)
        {
            _actions.Add(onValue);
        }
    }
}