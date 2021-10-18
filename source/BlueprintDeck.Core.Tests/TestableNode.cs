using System.Threading.Tasks;
using BlueprintDeck.Node;
using BlueprintDeck.Node.Ports;
using BlueprintDeck.Node.Properties;

namespace BlueprintDeck
{
    [Node("TestableNode", "TestableNode")]
    [ExcludeProperties]
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
        
        public Task<TTestData> ComplexInputReceiveTask => _tcsComplexInput.Task;
        
        [PortOptional]
        public IInput SimpleInput { get; set; }
        
        [PortOptional]
        public IInput<TTestData> ComplexInput { get; set; }
        
        [PortOptional]
        public IOutput SimpleOutput { get; set; }
        
        [PortOptional]
        public IOutput<TTestData> ComplexOutput { get; set; }
        
        [PropertyTitle("Test Property")]
        [IncludeProperty]
        public string TestProperty { get; set; }
        
        public TestableNode(TestableNodeAccessor<TTestData> nodeAccessor)
        {
            nodeAccessor.Node = this;
        }


        public Task Activate()
        {
            SimpleInput?.OnData(() =>
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