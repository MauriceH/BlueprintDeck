using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace BlueprintDeck.Node.Ports
{
    public class SimpleInput : IInput, IDisposable
    {
        private readonly IDisposable _subscription;
        private readonly List<Func<Task>> _actions = new List<Func<Task>>();


        public SimpleInput(IObservable<object> observable)
        {
            var connectable = observable.SelectMany(a => OnValueAsync()).Replay();
            _subscription = connectable.Connect();
        }
        
        
        private async Task<object> OnValueAsync()
        {
            var tasks = _actions.Select(x => x());
            await Task.WhenAll(tasks);
            return null;
        }
        
        public void Register(Func<Task> action)
        {
            _actions.Add(action);
        }


        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}