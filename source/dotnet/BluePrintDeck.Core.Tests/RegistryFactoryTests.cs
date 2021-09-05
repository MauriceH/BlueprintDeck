using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using BlueprintDeck.ConstantValue.Serializer;
using BlueprintDeck.Design.Registry;
using BlueprintDeck.Node.Default;
using BlueprintDeck.Node.Ports.Definitions;
using BlueprintDeck.Registration;
using NSubstitute;
using Xunit;
using Xunit.Sdk;

namespace BluePrintDeck.Core
{
    [ExcludeFromCodeCoverage]
    public class RegistryFactoryTests
    {
        [Fact]
        public void TestCreate_WhenCreate_ReturnsCorrectRegistry()
        {
            var testPort = new NodePortDefinition("Delay", "Delay", InputOutputType.Input, 2d, true);
            var testNode = new NodeRegistration("Delay", "Delay", typeof(DelayNode),
                new List<NodePortDefinition> { testPort });
            var testDataType = new DataTypeRegistration("double", typeof(double), "Double");

            var testConstantValue = new ConstantValueRegistration("CVR", "Value", typeof(double),
                new NodePortDefinition("out", "out", InputOutputType.Output, 45d, true),(context, func) => {});
            
            var nodeRegistrations = new[] { testNode };
            var dataTypeRegistrations = new[] { testDataType };
            var constantValueRegistrations = new [] {testConstantValue};


            var constantValueSerializerRepository = Substitute.For<IConstantValueSerializerRepository>();

            constantValueSerializerRepository.LoadSerializer(typeof(double)).Returns(new DoubleConstantValueSerializer());

            var sut = new RegistryFactory(nodeRegistrations, dataTypeRegistrations, constantValueRegistrations, constantValueSerializerRepository);
            var registry = sut.CreateNodeRegistry();

            Assert.NotNull(registry.DataTypes);
            Assert.Single(registry.DataTypes);
            var actualType = registry.DataTypes.First();
            Assert.Equal(testDataType.Key, actualType.Id);
            Assert.Equal(testDataType.Title, actualType.Title);
            Assert.Equal(testDataType.DataType.FullName, actualType.TypeName);

            Assert.NotNull(registry.NodeTypes);
            Assert.Single(registry.NodeTypes);
            var actualNode = registry.NodeTypes.First();
            Assert.Equal(testNode.Key, actualNode.Key);
            Assert.Equal(testNode.Title, actualNode.Title);

            Assert.NotNull(actualNode.Ports);
            Assert.Single(actualNode.Ports);
            var actualPort = actualNode.Ports.First();
            Assert.Equal(testPort.Key, actualPort.Key);
            Assert.Equal(testPort.Title, actualPort.Title);
            Assert.Equal(testPort.DataMode, actualPort.DataMode);
            Assert.Equal(testPort.Mandatory, actualPort.Mandatory);
            Assert.Equal(testPort.DefaultValue?.ToString(), actualPort.DefaultValue);
            Assert.Equal(testPort.InputOutputType, actualPort.InputOutputType);
            
            
            
            Assert.NotNull(registry.ConstantValueNodeTypes);
            Assert.Single(registry.ConstantValueNodeTypes);
            var actualValue = registry.ConstantValueNodeTypes.First();
            Assert.Equal(testConstantValue.Key, actualValue.Key);
            Assert.Equal(testConstantValue.Title, actualValue.Title);

            Assert.NotNull(actualValue.Port);
            var actualValuePort = actualValue.Port;
            Assert.Equal("value", actualValuePort.Key);
            Assert.Equal("Value", actualValuePort.Title);
            Assert.Null(actualValuePort.DefaultValue);
            
        }

        [Fact]
        public void TestCreateRegistry_WhenInvalidTypes_ThrowsException()
        {
            var testPort = new NodePortDefinition("Delay", "Delay", InputOutputType.Input, 2d, true);
            var testNode = new NodeRegistration("Delay", "Delay", typeof(DelayNode),
                new List<NodePortDefinition> { testPort });

            var testConstantValue = new ConstantValueRegistration("CVR", "Value", typeof(double),
                new NodePortDefinition("out", "out", InputOutputType.Output, 45d, true),(context, func) => {});
            
            var nodeRegistrations = new[] { testNode };
            var dataTypeRegistrations = new List<DataTypeRegistration>();
            var constantValueRegistrations = new [] {testConstantValue};

            var constantValueSerializerRepository = Substitute.For<IConstantValueSerializerRepository>();

            var sut = new RegistryFactory(nodeRegistrations, dataTypeRegistrations, constantValueRegistrations, constantValueSerializerRepository);

            Assert.Throws<Exception>(() =>
            {
                sut.CreateNodeRegistry();
            });
            nodeRegistrations = new NodeRegistration[] { };
            sut = new RegistryFactory(nodeRegistrations, dataTypeRegistrations, constantValueRegistrations, constantValueSerializerRepository);
            Assert.Throws<Exception>(() =>
            {
                sut.CreateNodeRegistry();
            });
        }

        [Fact]
        public void TestConstructor()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new RegistryFactory(new NodeRegistration[]{},new DataTypeRegistration[]{},new ConstantValueRegistration[] {},null);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new RegistryFactory(new NodeRegistration[]{},new DataTypeRegistration[]{},null,null);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new RegistryFactory(new NodeRegistration[]{},null,null,null);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new RegistryFactory(null,null,null,null);
            });
        }
    }
}