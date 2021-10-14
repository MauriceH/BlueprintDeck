using System;
using System.Threading.Tasks;

namespace BlueprintDeck.Node.Ports
{
    
    public interface IInput  : IPort
    {
        void OnData(Func<Task> action);
    }

    public interface IInput<out T> : IDisposable,  IPort
    {
        T? LastValue { get; }
        void OnData(Action<T> action);
        void OnData(Func<T, Task> onValue);
    }
}