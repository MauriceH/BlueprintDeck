using System;

namespace BlueprintDeck.Node.Ports
{
    public interface IOutput : IPort, IDisposable
    {
        void Emit();
    }

    public interface IOutput<in T> : IPort, IDisposable
    {
        void Emit(T data);
    }
}