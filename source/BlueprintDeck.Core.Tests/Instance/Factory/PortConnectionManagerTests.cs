using System.Collections.Generic;
using BlueprintDeck.Node.Default;
using BlueprintDeck.Node.Default.DataTypes;
using BlueprintDeck.Node.Ports;
using BlueprintDeck.Node.Ports.Registration;
using BlueprintDeck.Node.Properties.Registration;
using BlueprintDeck.Node.Registration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace BlueprintDeck.Instance.Factory
{
    public class PortConnectionManagerTests
    {
        [Fact]
        public void TestInitializePortAsOutput_WhenDoubleNodeOutput_InitializedSuccessfully()
        {
            var sut = new PortConnectionManager();

            var portRegistration = new PortRegistration(typeof(DoubleNode).GetProperty(nameof(DoubleNode.Output)), Direction.Output, typeof(double));
            var nodeRegistration = new NodeRegistration("1", "T", typeof(DoubleNode), new List<PortRegistration> { portRegistration },
                new List<string>(), new List<PropertyRegistration>());
            var nodeInstance = new NodeInstance("123", new Design.Node(), new DoubleNode(), nodeRegistration,
                new List<GenericTypeParameterInstance>());
            var portInstance = new PortInstance(portRegistration);

            sut.InitializePortAsOutput(nodeInstance, portInstance);
        }

        [Fact]
        public void TestInitializePortAsOutput_WhenSimpleNodeOutput_InitializedSuccessfully()
        {
            var sut = new PortConnectionManager();

            var portRegistration = new PortRegistration(typeof(ActivateNode).GetProperty(nameof(ActivateNode.Event)), Direction.Output);
            var nodeRegistration = new NodeRegistration("1", "T", typeof(ActivateNode), new List<PortRegistration> { portRegistration },
                new List<string>(), new List<PropertyRegistration>());
            var logger = Substitute.For<ILogger<ActivateNode>>();
            var nodeInstance = new NodeInstance("123", new Design.Node(), new ActivateNode(logger), nodeRegistration,
                new List<GenericTypeParameterInstance>());
            var portInstance = new PortInstance(portRegistration);

            sut.InitializePortAsOutput(nodeInstance, portInstance);
        }

        [Fact]
        public void TestInitializePortAsOutput_WhenNonGenericNode_FailsForGenericPort()
        {
            var sut = new PortConnectionManager();

            var portRegistration =
                new PortRegistration(typeof(DoubleNode).GetProperty(nameof(DoubleNode.Output)), Direction.Output, null, "TMissing");
            var nodeRegistration = new NodeRegistration("1", "T", typeof(DoubleNode), new List<PortRegistration> { portRegistration },
                new List<string>(), new List<PropertyRegistration>());
            var nodeInstance = new NodeInstance("123", new Design.Node(), new DoubleNode(), nodeRegistration,
                new List<GenericTypeParameterInstance>());
            var portInstance = new PortInstance(portRegistration);


            Assert.Throws<PortInitializationException>(() => { sut.InitializePortAsOutput(nodeInstance, portInstance); });
        }

        [Fact]
        public void TestInitializePortAsOutput_WhenGenericNode_FailsOnGenericTypeMismatch()
        {
            var sut = new PortConnectionManager();

            var portRegistration =
                new PortRegistration(typeof(DoubleNode).GetProperty(nameof(DoubleNode.Output)), Direction.Output, null, "TMissing");
            var nodeRegistration = new NodeRegistration("1", "T", typeof(DoubleNode), new List<PortRegistration> { portRegistration },
                new List<string> { "TDifferent" }, new List<PropertyRegistration>());
            var nodeInstance = new NodeInstance("123", new Design.Node(), new DoubleNode(), nodeRegistration,
                new List<GenericTypeParameterInstance> { new("TDifferent", typeof(double)) });
            var portInstance = new PortInstance(portRegistration);

            Assert.Throws<PortInitializationException>(() => { sut.InitializePortAsOutput(nodeInstance, portInstance); });
        }

        [Fact]
        public void TestInitializePortAsOutput_WhenGenericNode_FailsOnGenericTypeNull()
        {
            var sut = new PortConnectionManager();

            var portRegistration =
                new PortRegistration(typeof(DoubleNode).GetProperty(nameof(DoubleNode.Output)), Direction.Output, null, "TOK");
            var nodeRegistration = new NodeRegistration("1", "T", typeof(DoubleNode), new List<PortRegistration> { portRegistration },
                new List<string> { "TOK" }, new List<PropertyRegistration>());
            var nodeInstance = new NodeInstance("123", new Design.Node(), new DoubleNode(), nodeRegistration,
                new List<GenericTypeParameterInstance> { new("TOK", null) });
            var portInstance = new PortInstance(portRegistration);

            Assert.Throws<PortInitializationException>(() => { sut.InitializePortAsOutput(nodeInstance, portInstance); });
        }
    }
}