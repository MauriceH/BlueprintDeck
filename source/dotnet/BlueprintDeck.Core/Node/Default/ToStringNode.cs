using System.Collections.Generic;
using System.Threading.Tasks;
using BlueprintDeck.Node.Ports;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Node.Default
{
    [NodeDescriptor("ToString", "ToString", typeof(ToStringNodeDescriptor))]
    public class ToStringNode<TInput> : INode
    {
        public Task Activate(INodeContext nodeContext)
        {
            var input = nodeContext.GetPort<IInput<TInput>>(ToStringNodeDescriptor.Input);
            var output = nodeContext.GetPort<IOutput<string>>(ToStringNodeDescriptor.Output);
            input?.OnData(action: value =>
            {
                if (value == null || output == null) return;
                var stringData = value.ToString();
                if (stringData == null) return;
                output.Emit(stringData);
            });
            return Task.CompletedTask;
        }

        public Task Deactivate()
        {
            return Task.CompletedTask;
        }
    }

    public class ToStringNodeDescriptor : INodeDescriptor
    {
        public static NodePortDefinition Input => NodePortDefinitionFactory.CreateGenericDataInput("input", "Input", "TInput");
        public static NodePortDefinition Output => NodePortDefinitionFactory.CreateDataOutput<string>("output", "String");

        public IList<NodePortDefinition> PortDefinitions => new List<NodePortDefinition>() { Input, Output };
    }
}