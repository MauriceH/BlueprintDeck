using System.Collections.Generic;
using System.Threading.Tasks;
using BlueprintDeck.Node;
using BlueprintDeck.Node.Ports;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck
{
    [NodeDescriptor("TestableNode", "TestableNode", typeof(TestableNodeDescriptor))]
    public class TestableNode<TTestData> : INode
    {
        private readonly TaskCompletionSource _tcsActivationDone = new();
        private readonly TaskCompletionSource _tcsDeactivationDone = new();
        private readonly TaskCompletionSource _tcsSimpleInput = new();
        private readonly TaskCompletionSource<TTestData> _tcsComplexInput = new();
        
        public bool SimpleInputTriggered { get; private set; }
        public TTestData ComplexInputValue { get; private set; }
        public bool Activated { get; private set; }
        public bool Deactivated { get; private set; }

        public Task ActivationDoneTask => _tcsActivationDone.Task;
        public Task DeactivationDoneTask => _tcsDeactivationDone.Task;
        public Task SimpleInputReceiveTask => _tcsSimpleInput.Task;
        public Task ComplexInputReceiveTask => _tcsComplexInput.Task;
        
        public TestableNode(TestableNodeAccessor<TTestData> nodeAccessor)
        {
            nodeAccessor.Node = this;
        }


        public Task Activate(INodeContext nodeContext)
        {
            var simpleInput = nodeContext.GetPort<IInput>(TestableNodeDescriptor.SimpleInput);
            simpleInput?.Register(() =>
            {
                SimpleInputTriggered = true;
                _tcsSimpleInput.SetResult();
                return Task.CompletedTask;
            });
            var complexInput = nodeContext.GetPort<IInput<TTestData>>(TestableNodeDescriptor.ComplexInput);
            complexInput?.OnData(data =>
            {
                _tcsComplexInput.SetResult(data);
                ComplexInputValue = data;
            });
            Activated = true;
            _tcsActivationDone.SetResult();
            return Task.CompletedTask;
        }

        public Task Deactivate()
        {
            Deactivated = true;
            _tcsDeactivationDone.SetResult();
            return Task.CompletedTask;
        }
    }

    public class TestableNodeAccessor<T>
    {
        public TestableNode<T> Node { get; set; }
    }

    public class TestableNodeDescriptor : INodeDescriptor
    {
        public static readonly NodePortDefinition SimpleInput = NodePortDefinitionFactory.CreateInput("simple", "SimpleInput", false);

        public static readonly NodePortDefinition ComplexInput =
            NodePortDefinitionFactory.CreateGenericDataInput("simple", "SimpleInput", "TTestData",false);

        public IList<NodePortDefinition> PortDefinitions => new List<NodePortDefinition> { SimpleInput, ComplexInput };
    }
}