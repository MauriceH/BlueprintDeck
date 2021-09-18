using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using BlueprintDeck.ConstantValue.Registration;
using BlueprintDeck.ConstantValue.Serializer;
using BlueprintDeck.DataTypes.Registration;
using BlueprintDeck.Design.Registry;
using BlueprintDeck.Node.Default;
using BlueprintDeck.Node.Ports;
using BlueprintDeck.Node.Ports.Registration;
using BlueprintDeck.Node.Properties;
using BlueprintDeck.Node.Properties.Registration;
using BlueprintDeck.Node.Registration;
using NSubstitute;
using Xunit;

namespace BlueprintDeck
{
    [ExcludeFromCodeCoverage]
    public class RegistryFactoryTests
    {
        [Fact]
        public void TestCreate_WhenCreate_ReturnsCorrectRegistry()
        {
            var testPort = new PortRegistration("Delay", Direction.Input,typeof(double)) {Mandatory = true};
            var testNode = new NodeRegistration("Delay", "Delay", typeof(DelayNode),
                new List<PortRegistration> { testPort }, new List<string>(), new List<PropertyRegistration>());
            var testDataType = new DataTypeRegistration("double", typeof(double), "Double");

            var testConstantValue = new ConstantValueRegistration("CVR", "Value", typeof(double),
                new PortRegistration("out", Direction.Output, typeof(double)) {Title="out", Mandatory = true}, (func) => { });

            var nodeRegistrations = new[] { testNode };
            var dataTypeRegistrations = new[] { testDataType };
            var constantValueRegistrations = new[] { testConstantValue };


            var constantValueSerializerRepository = Substitute.For<IConstantValueSerializerRepository>();

            constantValueSerializerRepository.LoadSerializer(typeof(double)).Returns(new DoubleConstantValueSerializer());

            var sut = new RegistryFactory(nodeRegistrations, dataTypeRegistrations, constantValueRegistrations);
            var registry = sut.CreateNodeRegistry();

            Assert.NotNull(registry.DataTypes);
            Assert.Single(registry.DataTypes);
            var actualType = registry.DataTypes.First();
            Assert.Equal(testDataType.Id, actualType.Id);
            Assert.Equal(testDataType.Title, actualType.Title);
            Assert.Equal(testDataType.DataType.FullName, actualType.TypeName);

            Assert.NotNull(registry.NodeTypes);
            Assert.Single(registry.NodeTypes);
            var actualNode = registry.NodeTypes.First();
            Assert.Equal(testNode.Id, actualNode.Id);
            Assert.Equal(testNode.Title, actualNode.Title);

            Assert.NotNull(actualNode.Ports);
            Assert.Single(actualNode.Ports);
            var actualPort = actualNode.Ports.First();
            Assert.Equal(testPort.Key, actualPort.Key);
            Assert.Equal(testPort.Title, actualPort.Title);
            Assert.Equal(testPort.Mandatory, actualPort.Mandatory);
            Assert.Equal(testPort.Direction, actualPort.Direction);


            Assert.NotNull(registry.ConstantValueNodeTypes);
            Assert.Single(registry.ConstantValueNodeTypes);
            var actualValue = registry.ConstantValueNodeTypes.First();
            Assert.Equal(testConstantValue.Key, actualValue.Id);
            Assert.Equal(testConstantValue.Title, actualValue.Title);

            Assert.NotNull(actualValue.Port);
            var actualValuePort = actualValue.Port;
            Assert.Equal("value", actualValuePort.Key);
            Assert.Equal("Value", actualValuePort.Title);
        }

        [Fact]
        public void TestCreateRegistry_WhenGenericNode_OutputsGenericParameters()
        {
            var expectedTypeName = "TType";
            var nodeRegistration = new NodeRegistration("id", "title", typeof(ToStringNode<>), new List<PortRegistration>(),
                new List<string> { expectedTypeName }, new List<PropertyRegistration>());
            var sut = new RegistryFactory(new[] { nodeRegistration }, new List<DataTypeRegistration>(), new List<ConstantValueRegistration>());

            var actual = sut.CreateNodeRegistry();
            Assert.NotNull(actual.NodeTypes);
            Assert.NotEmpty(actual.NodeTypes);
            var nodeType = actual.NodeTypes.ToList()[0];
            Assert.NotNull(nodeType.GenericTypes);
            Assert.NotEmpty(nodeType.GenericTypes);
            Assert.Equal(expectedTypeName, nodeType.GenericTypes[0]);
        }

        [Fact]
        public void TestCreateRegistry_WhenInvalidTypes_ThrowsException()
        {
            var testPort = new PortRegistration("Delay", Direction.Input, typeof(double)) { Title = "Delay", Mandatory = true};
            var testNode = new NodeRegistration("Delay", "Delay", typeof(DelayNode),
                new List<PortRegistration> { testPort }, new List<string>(), new List<PropertyRegistration>());

            var testConstantValue = new ConstantValueRegistration("CVR", "Value", typeof(double),
                new PortRegistration("out", Direction.Output, typeof(double)){ Title = "out", Mandatory = true}, (func) => { });

            var nodeRegistrations = new[] { testNode };
            var dataTypeRegistrations = new List<DataTypeRegistration>();
            var constantValueRegistrations = new[] { testConstantValue };

            var constantValueSerializerRepository = Substitute.For<IConstantValueSerializerRepository>();

            var sut = new RegistryFactory(nodeRegistrations, dataTypeRegistrations, constantValueRegistrations);

            Assert.Throws<Exception>(() => { sut.CreateNodeRegistry(); });
            nodeRegistrations = new NodeRegistration[] { };
            sut = new RegistryFactory(nodeRegistrations, dataTypeRegistrations, constantValueRegistrations);
            Assert.Throws<Exception>(() => { sut.CreateNodeRegistry(); });
        }

        [Fact]
        public void TestConstructor()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new RegistryFactory(new NodeRegistration[] { }, new DataTypeRegistration[] { }, null);
            });
            Assert.Throws<ArgumentNullException>(() => { new RegistryFactory(new NodeRegistration[] { }, null, null); });
            Assert.Throws<ArgumentNullException>(() => { new RegistryFactory(null, null, null); });
        }
    }
}