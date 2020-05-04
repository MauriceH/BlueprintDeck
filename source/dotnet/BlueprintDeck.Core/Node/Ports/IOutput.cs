namespace BlueprintDeck.Node.Ports
{
    public interface IOutput : IPortInputOutput
    {
        void Emit();
    }

    public interface IOutput<in T> : IPortInputOutput
    {
        void Emit(T data);
    }
}