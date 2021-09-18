using System.Threading.Tasks;
using BlueprintDeck.Node;
using BlueprintDeck.Node.Ports;
using BlueprintDeck.Node.Properties;

namespace BlueprintDeck
{
    [Node("TestableNode", "TestableNode")]
    public class TestableNode<TTestData> : INode
    {
        private readonly TaskCompletionSource _tcsActivationDone = new();
        private readonly TaskCompletionSource _tcsDeactivationDone = new();
        private readonly TaskCompletionSource _tcsSimpleInput = new();
        private readonly TaskCompletionSource<TTestData> _tcsComplexInput = new();
        
        [ExcludeFromProperties]
        public bool SimpleInputTriggered { get; private set; }
        
        [ExcludeFromProperties]
        public TTestData ComplexInputValue { get; private set; }
        
        [ExcludeFromProperties]
        public bool Activated { get; private set; }
        
        [ExcludeFromProperties]
        public bool Deactivated { get; private set; }

        [ExcludeFromProperties]
        public Task ActivationDoneTask => _tcsActivationDone.Task;
        
        [ExcludeFromProperties]
        public Task DeactivationDoneTask => _tcsDeactivationDone.Task;
        
        [ExcludeFromProperties]
        public Task SimpleInputReceiveTask => _tcsSimpleInput.Task;
        
        [ExcludeFromProperties]
        public Task ComplexInputReceiveTask => _tcsComplexInput.Task;
        
        [PortOptional]
        public IInput SimpleInput { get; set; }
        
        [PortOptional]
        public IInput<TTestData> ComplexInput { get; set; }
        
        
        public TestableNode(TestableNodeAccessor<TTestData> nodeAccessor)
        {
            nodeAccessor.Node = this;
        }


        public Task Activate()
        {
            SimpleInput?.Register(() =>
            {
                SimpleInputTriggered = true;
                _tcsSimpleInput.SetResult();
                return Task.CompletedTask;
            });
            ComplexInput?.OnData(data =>
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

}