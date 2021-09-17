using System.Collections.Generic;
using System.Threading.Tasks;
using BlueprintDeck.Design;
using BlueprintDeck.Node.Ports.Definitions;
using BlueprintDeck.Registration;
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
            
            var cvSerializerRepository = Substitute.For<IConstantValueSerializerRepository>();
            var nodeFactory = Substitute.For<INodeFactory>();

            var testableNodeAccessor = new TestableNodeAccessor<double>();
            
            var node = new TestableNode<double>(testableNodeAccessor);
            
            var nodeRegistration = new NodeRegistration("TestableNode","TestableNode",typeof(TestableNode<>),new List<NodePortDefinition>(),new List<string> {"TTestData"});
            var createResult = new CreateNodeResult<NodeRegistration>(nodeRegistration,node);
            nodeFactory.CreateNode(Arg.Any<IServiceScope>(), Arg.Any<string>(), Arg.Any<Design.Node>())
                .Returns(createResult);
            
            var sut = new BlueprintFactory(provider,nodeFactory,cvSerializerRepository);

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