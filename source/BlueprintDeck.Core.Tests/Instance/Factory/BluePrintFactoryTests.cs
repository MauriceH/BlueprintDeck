using System.Collections.Generic;
using System.Threading.Tasks;
using BlueprintDeck.ConstantValue.Registration;
using BlueprintDeck.Design;
using BlueprintDeck.Node.Ports.Registration;
using BlueprintDeck.Node.Properties;
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
            
            var cvSerializerRepository = Substitute.For<IValueSerializerRepository>();
            var nodeFactory = Substitute.For<INodeFactory>();

            var testableNodeAccessor = new TestableNodeAccessor<double>();
            
            var node = new TestableNode<double>(testableNodeAccessor);
            
            var nodeRegistration = new NodeRegistration("TestableNode","TestableNode",typeof(TestableNode<>),new List<PortRegistration>(),new List<string> {"TTestData"},new List<PropertyRegistration>());
            var createResult = new CreateNodeResult(nodeRegistration,node,new List<GenericTypeParameterInstance>() {new("TTestData",typeof(string))});
            nodeFactory.CreateNode(Arg.Any<IServiceScope>(), Arg.Any<string>(), Arg.Any<Design.Node>())
                .Returns(createResult);
            
            var sut = new BlueprintFactory(provider,nodeFactory, new PortInstanceFactory());

            var blueprint = new Design.Blueprint
            {
                Nodes = new List<Design.Node>
                {
                    new Design.Node
                    {
                        Id = "001",
                        Title = "Test",
                        GenericTypes = new List<NodeGenericType>() {new() {GenericParameter = "TTestData", TypeId = "T1"}},
                        NodeTypeKey = "TestableNode"
                    }
                }
            };
            var blueprintInstance = sut.CreateBlueprint(blueprint);
            
            blueprintInstance.Activate();

            await testableNodeAccessor.Node.ActivationDoneTask;
            
            blueprintInstance.Dispose();

            await testableNodeAccessor.Node.DeactivationDoneTask;

        }
        
    }
}