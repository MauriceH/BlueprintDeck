using BlueprintDeck.Node.Ports.Definitions.DataTypes;

namespace BlueprintDeck.Node.Ports
{
    public interface IOutput
    {
        void Emit();
    }

    public interface IOutput<in T>  where T: IPortData
    {
        void Emit(T data);
    }
}