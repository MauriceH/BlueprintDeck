using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using BlueprintDeck.Misc;
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
        private readonly NodeRegistrationFactory _sut = new(new AssemblyTypesResolver(), new PortRegistrationFactory(),
            new GenericTypeParametersFactory(), new PropertyRegistrationFactory());

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
            Assert.Null(_sut.CreateNodeRegistration(typeof(int)));
            Assert.Null(_sut.CreateNodeRegistration(typeof(MissingInterfaceNode)));
        }

        [Fact]
        public void TestCreate_WhenNodeWithoutIdAttribute_GeneratesHashId()
        {
            var registration = _sut.CreateNodeRegistration(typeof(MissingIdNode));
            Assert.NotNull(registration.Id);
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

        [Node("id", "title")]
        private class MissingInterfaceNode
        {
        }


        [Node(null!, "title")]
        private class MissingIdNode : INode
        {
            public Task Activate(INodeContext nodeContext)
            {
                throw new NotImplementedException();
            }

            public Task Deactivate()
            {
                throw new NotImplementedException();
            }
        }
    }
}