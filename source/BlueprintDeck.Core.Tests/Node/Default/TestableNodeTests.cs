using System.Linq;
using BlueprintDeck.DependencyInjection;
using BlueprintDeck.Node.Registration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlueprintDeck.Node.Default
{
    public class TestableNodeTests
    {
        [Fact]
        public void TestTestableNode()
        {
            var services = new ServiceCollection();
            services.AddBlueprintDeck(config =>
            {
                config.RegisterNode(typeof(TestableNode<>));
            });

            var provider = services.BuildServiceProvider();

            var nodeRegistrations = provider.GetServices<NodeRegistration>().ToList();

            Assert.NotEmpty(nodeRegistrations);
            
            var nodeRegistration = nodeRegistrations.FirstOrDefault(x => x.Id == "TestableNode");
            
            Assert.NotNull(nodeRegistration);
            
            Assert.Equal("TestableNode",nodeRegistration.Id);
            Assert.Equal("TestableNode",nodeRegistration.Title);
            Assert.NotEmpty(nodeRegistration.Ports);
            Assert.Equal(4,nodeRegistration.Ports.Count);
            Assert.NotEmpty(nodeRegistration.Properties);
            Assert.Equal(1,nodeRegistration.Properties.Count);
            var property = nodeRegistration.Properties.First();
            Assert.NotNull(property);
            Assert.Equal("TestProperty", property.Name);
            Assert.Equal("Test Property", property.Title);
            Assert.Equal(typeof(string), property.Type);
        }
    }
}