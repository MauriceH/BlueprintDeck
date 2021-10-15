using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlueprintDeck.DataTypes.Registration;
using BlueprintDeck.Design;
using BlueprintDeck.Node;
using BlueprintDeck.Node.Default;
using BlueprintDeck.Node.Ports.Registration;
using BlueprintDeck.Node.Properties.Registration;
using BlueprintDeck.Node.Registration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace BlueprintDeck.Instance.Factory
{
    public class NodeFactoryTests
    {
        
        [Fact]
        public void TestNodeFactory()
        {
            var scope = Substitute.For<IServiceScope>();
            var nodeRegistration = new NodeRegistration("123"
                , "Title"
                , typeof(TestNode), new List<PortRegistration>(), new List<string>(),
                new List<PropertyRegistration>());
            var sut = new NodeFactory(new[] { nodeRegistration }, new DataTypeRegistration[] { });
            var createResult = sut.CreateNode(scope, "123", new Design.Node
            {
                Id = "ADS",
            });
            Assert.True(createResult.Node is TestNode);
        }
        
        [Fact]
        public void TestNodeFactory_WhenGivenType_NotFoundInRegistrations()
        {
            var scope = Substitute.For<IServiceScope>();
            var sut = new NodeFactory(new NodeRegistration[] { }, new DataTypeRegistration[] { });
            Assert.Throws<Exception>(() => { sut.CreateNode(scope, "asd", new Design.Node()); });
        }

        [Fact]
        public void TestNodeFactory_WhenGenericTypes_NotMatchClassAndRegistration()
        {
            var scope = Substitute.For<IServiceScope>();
            var nodeRegistration = new NodeRegistration("123"
                , "Title"
                , typeof(ToStringNode<>), new List<PortRegistration>(), new List<string>(),
                new List<PropertyRegistration>());
            var sut = new NodeFactory(new[] { nodeRegistration }, new DataTypeRegistration[] { });
            Assert.Throws<Exception>(() => { sut.CreateNode(scope, "123", new Design.Node()); });
        }



        [Fact]
        public void TestNodeFactory_WhenGenericTypeInstanceTypeId_NotFoundInDataTypeRegistrations()
        {
            var scope = Substitute.For<IServiceScope>();
            var nodeRegistration = new NodeRegistration("123"
                , "Title"
                , typeof(ToStringNode<>), new List<PortRegistration>(), new List<string>(),
                new List<PropertyRegistration>());
            var sut = new NodeFactory(new[] { nodeRegistration }, new DataTypeRegistration[] { });
            Assert.Throws<Exception>(() =>
            {
                sut.CreateNode(scope, "123", new Design.Node
                {
                    Id = "ADS",
                    GenericTypes = new List<NodeGenericType>
                    {
                        new()
                        {
                            GenericParameter = "TInput",
                            TypeId = "123"
                        }
                    }
                });
            });
        }


        [Fact]
        public void TestNodeFactory_WhenGenericTypeInstanceTypeId_NotFoundInDataTypeRegistrations2()
        {
            var scope = Substitute.For<IServiceScope>();
            var nodeRegistration = new NodeRegistration("123"
                , "Title"
                , typeof(ToStringNode<>), new List<PortRegistration>(), new List<string>(),
                new List<PropertyRegistration>());
            var sut = new NodeFactory(new[] { nodeRegistration }, new DataTypeRegistration[] { });
            Assert.Throws<Exception>(() =>
            {
                sut.CreateNode(scope, "123", new Design.Node
                {
                    Id = "ADS",
                    GenericTypes = new List<NodeGenericType>
                    {
                        new()
                        {
                            GenericParameter = "ASD",
                            TypeId = "123"
                        }
                    }
                });
            });
        }

        [Node]
        private class TestNode : INode
        {
            public TestNode(Design.Node node)
            {
                
            }

            public Task Activate()
            {
                return Task.CompletedTask;
            }

            public Task Deactivate()
            {
                return Task.CompletedTask;
            }
        }
    }
}