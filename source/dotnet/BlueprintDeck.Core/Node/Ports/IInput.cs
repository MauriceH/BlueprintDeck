using System;
using System.Threading.Tasks;

namespace BlueprintDeck.Node.Ports
{
    
    public interface IInput
    {
        void Register(Func<Task> action);
    }

    public interface IInput<out T> : IDisposable
    {
        T Value { get; }
        void OnData(Action<T> action);
    }
}