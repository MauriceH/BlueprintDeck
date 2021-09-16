using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using BlueprintDeck.Node;
using BlueprintDeck.Node.Default;
using BlueprintDeck.Node.Ports.Definitions;
using BlueprintDeck.Registration;
using Xunit;

namespace BlueprintDeck
{
    [ExcludeFromCodeCoverage]
    public class NodeRegistrationResolverTests
    {
        private readonly NodeRegistrationFactory _sut = new();

        [Fact]
        public void TestCreateByAssembly_WhenGivenAssembly_ResolvesRegistrations()
        {
            var registrations = _sut.CreateNodeRegistrationsByAssembly(_sut.GetType().Assembly);
            Assert.NotNull(registrations);
            var nodeRegistrations = registrations.ToList();
            Assert.Contains(nodeRegistrations, registration => registration.NodeType == typeof(ActivateNode));
        }
        
        [Fact]
        public void TestCreateByAssembly_WhenAssemblyWithInvalidNodes_ResolvesRegistrations()
        {
            var registrations = _sut.CreateNodeRegistrationsByAssembly(typeof(NodeRegistrationResolverTests).Assembly);
            Assert.NotNull(registrations);
            var nodeRegistrations = registrations.ToList();
            Assert.NotEmpty(nodeRegistrations);
        }
        
        [Fact]
        public void TestCreate_WhenInvalidTypes_ReturnNullOrThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.CreateNodeRegistration(null!));
            Assert.Throws<ArgumentException>(() => _sut.CreateNodeRegistration(typeof(MissingDescriptorNode)!));
            Assert.Null(_sut.CreateNodeRegistration(typeof(int)));
            Assert.Null(_sut.CreateNodeRegistration(typeof(MissingInterfaceNode)));
        }
        
        [Fact]
        public void TestCreate_WhenNodeWithoutIdAttribute_GeneratesHashId()
        {
            var registration = _sut.CreateNodeRegistration(typeof(MissingIdNode));
            Assert.NotNull(registration.Key);
        }
        
        [Fact]
        public void TestCreate_WhenGenericNodeType_ReturnsCorrectNodeRegistration()
        {
            var registration = _sut.CreateNodeRegistration<ActivateNode>();
            Assert.True(registration.NodeType.Equals(typeof(ActivateNode)));
        }
        
        [Fact]
        public void TestCreate_WhenInvalidType_ThrowsException()
        {
            
        }

        [NodeDescriptor("id", "title", typeof(Descriptor))]
        private class MissingInterfaceNode
        {
        }

        [NodeDescriptor(null!, "title", null!)]
        private class MissingDescriptorNode : INode
        {
            public Design.Node DesignValues { get; set; }
            public Task Activate(INodeContext nodeContext)
            {
                throw new NotImplementedException();
            }

            public Task Deactivate()
            {
                throw new NotImplementedException();
            }
        }
        
        [NodeDescriptor(null!, "title", typeof(Descriptor))]
        private class MissingIdNode : INode
        {
            public Design.Node DesignValues { get; set; }
            public Task Activate(INodeContext nodeContext)
            {
                throw new NotImplementedException();
            }

            public Task Deactivate()
            {
                throw new NotImplementedException();
            }
        }
        
        
        public class Descriptor : INodeDescriptor
        {
            public IList<NodePortDefinition> PortDefinitions => new List<NodePortDefinition>();
        }
      
    }
}