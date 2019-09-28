using System;
using System.Collections.Generic;
using BlueprintDeck.Node.Ports.Definitions.DataTypes;

namespace BlueprintDeck.Node.Ports
{
    public class DataInput<T> : IInput<T> where T : IPortData
    {
        private readonly IDisposable _subscription;
        private readonly List<Action<T>> _actions = new List<Action<T>>();
        private T _lastValue;
        
        public DataInput(IObservable<T> observable)
        {
            _subscription = observable.Subscribe(OnValue);
        }

        private void OnValue(T value)
        {
            _lastValue = value;
            foreach (var action in _actions)
            {
                action(value);
            }
        }
        
        public void Dispose()
        {
            _subscription?.Dispose();
        }

        public T Value => _lastValue;
        
        public void Register(Action<T> onValue)
        {
            _actions.Add(onValue);
        }
    }
}