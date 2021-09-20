using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace BlueprintDeck.Node.Ports
{
    public class DataInput<T> : IInput<T>
    {
        private readonly IDisposable _subscription;
        private Func<T, Task>? _action = null;

        public DataInput(IObservable<T> observable)
        {
            var connectable = observable.SelectMany(OnValueAsync).Replay();
            _subscription = connectable.Connect();
        }

        private async Task<T> OnValueAsync(T value)
        {
            LastValue = value;
            var task = _action?.Invoke(value);
            if (task != null) await task;
            return value;
        }
        
        public void Dispose()
        {
            _subscription.Dispose();
        }

        public T? LastValue { get; private set; }

        public void OnData(Action<T> onValue)
        {
            _action = value =>
            {
                onValue(value);
                return Task.CompletedTask;
            };
        }

        public void OnData(Func<T, Task> onValue)
        {
            _action = onValue;
        }
    }
}