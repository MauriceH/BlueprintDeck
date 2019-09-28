using System.Threading.Tasks;
using BlueprintDeck.Node.Ports;
using BlueprintDeck.Node.Ports.Definitions.DataTypes;

namespace BlueprintDeck.Node.ValueNode
{
    public abstract class ValueDataNode<T> : INode, ISimpleDataNode<T> where T : IPortData
    {
        private T _value;
        public string ShortTitle { get; set; }
        private IOutput<T> Port { get; set; }
        
        public void ChangeValue(T data)
        {
            _value = data;
            Port?.Emit(_value);
        }
        
        public Task Activate(INodeContext nodeContext)
        {
            Port = nodeContext.GetPort<IOutput<T>>(ValueDataNodeDescriptor<T>.Definition);
            Port.Emit(_value);
            return Task.CompletedTask;
        }

        public Task Deactivate()
        {
            return Task.CompletedTask;
        }
    }
}