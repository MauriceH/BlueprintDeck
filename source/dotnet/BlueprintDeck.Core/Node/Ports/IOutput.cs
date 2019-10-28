namespace BlueprintDeck.Node.Ports
{
    public interface IOutput
    {
        void Emit();
    }

    public interface IOutput<in T>
    {
        void Emit(T data);
    }
}