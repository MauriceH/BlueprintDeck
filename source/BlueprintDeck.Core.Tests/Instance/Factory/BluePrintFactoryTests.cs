using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlueprintDeck.ConstantValue.Registration;
using BlueprintDeck.Design;
using BlueprintDeck.Node.Ports;
using BlueprintDeck.Node.Ports.Registration;
using BlueprintDeck.Node.Properties.Registration;
using BlueprintDeck.Node.Registration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace BlueprintDeck.Instance.Factory
{
    public class BluePrintFactoryTests
    {
        [Fact]
        public async Task TestFactory()
        {
            var services = new ServiceCollection();


            services.AddSingleton(Substitute.For<ILogger<Blueprint>>());

            var provider = services.BuildServiceProvider();

            var nodeFactory = Substitute.For<INodeFactory>();

            var testableNodeAccessor1 = new TestableNodeAccessor<string>();
            var testableNodeAccessor2 = new TestableNodeAccessor<string>();


            var nodeRegistration = new NodeRegistration("TestableNode", "TestableNode", typeof(TestableNode<>), new List<PortRegistration>
                {
                    new(typeof(TestableNode<>).GetProperty("ComplexInput"), Direction.Input, null, "TTestData") {Mandatory = false},
                    new(typeof(TestableNode<>).GetProperty("ComplexOutput"), Direction.Output, null, "TTestData") {Mandatory = false},
                },
                new List<string> { "TTestData" }, new List<PropertyRegistration>());


            var createResult1 = new CreateNodeResult(nodeRegistration, new TestableNode<string>(testableNodeAccessor1),
                new List<GenericTypeParameterInstance> { new("TTestData", typeof(string)) });
            var createResult2 = new CreateNodeResult(nodeRegistration, new TestableNode<string>(testableNodeAccessor2),
                new List<GenericTypeParameterInstance> { new("TTestData", typeof(string)) });


            var sut = new BlueprintFactory(provider, nodeFactory, new PortInstanceFactory());

            Design.Node node1 = new()
            {
                Id = "001",
                Title = "Test1",
                GenericTypes = new List<NodeGenericType> { new() { GenericParameter = "TTestData", TypeId = "T1" } },
                NodeTypeKey = "TestableNode",
                Location = new(100, 200),
                Properties = new Dictionary<string, string>
                {
                    ["TestProperty"] = "Test1"
                }
            };
            Design.Node node2 = new()
            {
                Id = "002",
                Title = "Test2",
                GenericTypes = new List<NodeGenericType> { new() { GenericParameter = "TTestData", TypeId = "T1" } },
                NodeTypeKey = "TestableNode",
                Location = new(300, 500),
                Properties = new Dictionary<string, string>
                {
                    ["TestProperty"] = "Test2"
                }
            };
            var blueprint = new Design.Blueprint
            {
                Nodes = new List<Design.Node>
                {
                    node1,
                    node2
                },
                Connections = new List<Connection>
                {
                    new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        NodeFrom = "001",
                        NodePortFrom = "ComplexOutput",
                        NodeTo = "002",
                        NodePortTo = "ComplexInput"
                    }
                }
            };


            nodeFactory.CreateNode(Arg.Any<IServiceScope>(), Arg.Any<string>(), Arg.Is(node1))
                .Returns(createResult1);
            nodeFactory.CreateNode(Arg.Any<IServiceScope>(), Arg.Any<string>(), Arg.Is(node2))
                .Returns(createResult2);

            var blueprintInstance = sut.CreateBlueprint(blueprint);

            blueprintInstance.Activate();

            await testableNodeAccessor1.Node.ActivationDoneTask;
            await testableNodeAccessor2.Node.ActivationDoneTask;


            testableNodeAccessor1.Node.ComplexOutput.Emit("TestValue");

            var portResult = await testableNodeAccessor2.Node.ComplexInputReceiveTask;
            Assert.Equal("TestValue", portResult);

            blueprintInstance.Dispose();

            await testableNodeAccessor1.Node.DeactivationDoneTask;
            await testableNodeAccessor2.Node.DeactivationDoneTask;
        }
    }
}