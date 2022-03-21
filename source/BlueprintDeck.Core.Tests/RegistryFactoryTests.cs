using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using BlueprintDeck.ConstantValue.Serializer;
using BlueprintDeck.DataTypes.Registration;
using BlueprintDeck.Design.Registry;
using BlueprintDeck.Node.Default;
using BlueprintDeck.Node.Ports;
using BlueprintDeck.Node.Ports.Registration;
using BlueprintDeck.Node.Properties.Registration;
using BlueprintDeck.Node.Registration;
using BlueprintDeck.ValueSerializer.Registration;
using BlueprintDeck.ValueSerializer.Serializer;
using NSubstitute;
using Xunit;

namespace BlueprintDeck
{
    [ExcludeFromCodeCoverage]
    public class RegistryFactoryTests
    {
        public IInput<TimeSpan> Delay { get; set; }

        public PropertyInfo Property => this.GetType().GetProperties().First();

        [Fact]
        public void TestCreate_WhenCreate_ReturnsCorrectRegistry()
        {
            var typedTestPort = new PortRegistration(Property, Direction.Input, typeof(double)) { Mandatory = true };
            var genericTestPort = new PortRegistration(Property, Direction.Input, typeof(double), "Test1") { Mandatory = true };
            var testNode = new NodeRegistration("Delay", "Delay", typeof(DelayNode),
                new List<PortRegistration> { genericTestPort, typedTestPort }, new List<string>() { "Test1" }, new List<PropertyRegistration>()
                {
                    new(typeof(DelayNode).GetProperty("DefaultDelay")) { Title = "Default Delay" }
                });
            var testDataType = new DataTypeRegistration("double", typeof(double), "Double");
            var testDataType2 = new DataTypeRegistration("TimeSpan", typeof(TimeSpan?), "TimeSpan");

            var nodeRegistrations = new List<NodeRegistration> { testNode };
            var dataTypeRegistrations = new List<DataTypeRegistration> { testDataType, testDataType2 };


            var constantValueSerializerRepository = Substitute.For<IValueSerializerRepository>();

            constantValueSerializerRepository.LoadSerializer(typeof(double)).Returns(new DoubleValueSerializer());

            var sut = new BlueprintDeckRegistryFactory(nodeRegistrations, dataTypeRegistrations);
            var registry = sut.CreateNodeRegistry();

            Assert.NotNull(registry.DataTypes);
            Assert.Equal(2, registry.DataTypes.Count());
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
            Assert.Equal(2, actualNode.Ports.Count());
            var actualGenericPort = actualNode.Ports.First();
            Assert.Equal(genericTestPort.Key, actualGenericPort.Key);
            Assert.Equal(genericTestPort.Title, actualGenericPort.Title);
            Assert.Equal(genericTestPort.Mandatory, actualGenericPort.Mandatory);
            Assert.Equal(genericTestPort.Direction, actualGenericPort.Direction);
            Assert.Equal(genericTestPort.GenericTypeParameter, actualGenericPort.GenericTypeParameter);


            var actualTypedPort = actualNode.Ports.Last();
            Assert.Equal(testDataType.Id, actualTypedPort.TypeId);

            Assert.NotNull(actualNode.Properties);
            Assert.Single(actualNode.Properties);
            var actualProperty = actualNode.Properties.First();
            Assert.NotNull(actualProperty);
            Assert.Equal("Default Delay", actualProperty.Title);
            Assert.Equal("DefaultDelay", actualProperty.Name);
            Assert.Equal(testDataType2.Id, actualProperty.TypeId);

            Assert.NotNull(actualNode.GenericTypes);
            Assert.Single(actualNode.GenericTypes);
            Assert.Equal("Test1", actualNode.GenericTypes[0]);
        }

        [Fact]
        public void TestCreateRegistry_WhenGenericNode_OutputsGenericParameters()
        {
            var expectedTypeName = "TType";
            var nodeRegistration = new NodeRegistration("id", "title", typeof(ToStringNode<>), new List<PortRegistration>(),
                new List<string> { expectedTypeName }, new List<PropertyRegistration>());
            var sut = new BlueprintDeckRegistryFactory(new List<NodeRegistration> { nodeRegistration }, new List<DataTypeRegistration>());

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
            var testPort = new PortRegistration(Property, Direction.Input, typeof(double)) { Title = "Delay", Mandatory = true };
            var testNode = new NodeRegistration("Delay", "Delay", typeof(DelayNode),
                new List<PortRegistration> { testPort }, new List<string>(), new List<PropertyRegistration>());


            var nodeRegistrations = new List<NodeRegistration> { testNode };
            var dataTypeRegistrations = new List<DataTypeRegistration>();

            var sut = new BlueprintDeckRegistryFactory(nodeRegistrations, dataTypeRegistrations);

            Assert.Throws<Exception>(() => { sut.CreateNodeRegistry(); });
            nodeRegistrations = new List<NodeRegistration> { };
            sut = new BlueprintDeckRegistryFactory(nodeRegistrations, dataTypeRegistrations);
        }

        [Fact]
        public void TestConstructor()
        {
            Assert.Throws<ArgumentNullException>(() => { new BlueprintDeckRegistryFactory(new List<NodeRegistration>(), null); });
            Assert.Throws<ArgumentNullException>(() => { new BlueprintDeckRegistryFactory(null, null); });
        }

        [Fact]
        public void TestCreateRegistry_WhenMissingPropertyDataType_ThrowsException()
        {
            var nodeRegistration = new NodeRegistration("id", "title", typeof(DelayNode), new List<PortRegistration>(),
                new List<string>(), new List<PropertyRegistration>() { new(typeof(DelayNode).GetProperty("DefaultDelay")) });
            var sut = new BlueprintDeckRegistryFactory(new List<NodeRegistration> { nodeRegistration }, new List<DataTypeRegistration>());

            Assert.Throws<Exception>(() => { sut.CreateNodeRegistry(); });
        }
    }
}