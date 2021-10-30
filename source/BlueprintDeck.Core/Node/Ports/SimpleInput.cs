using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace BlueprintDeck.Node.Ports
{
    public class SimpleInput : IInput, IDisposable
    {
        private readonly IObservable<object> _rootObservable;

        public IObservable<object> Observable => _rootObservable.AsObservable();

        public SimpleInput(IObservable<object> rootObservable)
        {
            _rootObservable = rootObservable;
        }

        public IDisposable Subscribe(Func<Task> action)
        {
            var connectable = _rootObservable.SelectMany(async _ =>
            {
                await action();
                return _;
            }).Replay();
            return connectable.Connect();
        }

        public void Dispose()
        {
        }
    }
}