using System;
using System.Threading.Tasks;
using BlueprintDeck.Node.Ports.Definitions.DataTypes;

namespace BlueprintDeck.Node.Ports
{
    
    public interface IInput
    {
        void Register(Func<Task> action);
    }

    public interface IInput<out T> : IDisposable where T: IPortData
    {
        T Value { get; }
        void Register(Action<T> action);
    }
}