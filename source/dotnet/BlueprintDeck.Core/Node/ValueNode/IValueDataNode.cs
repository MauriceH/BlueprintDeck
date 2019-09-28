using BlueprintDeck.Node.Ports.Definitions.DataTypes;

namespace BlueprintDeck.Node.ValueNode
{
    public interface ISimpleDataNode<T> where T : IPortData
    {
        void ChangeValue(T data);
    }
}