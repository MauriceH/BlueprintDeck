using System;
using System.Linq;
using BlueprintDeck.ConstantValue.Registration;
using BlueprintDeck.DataTypes.Registration;
using BlueprintDeck.Design.Registry;
using BlueprintDeck.Instance.Factory;
using BlueprintDeck.Node.Default.DataTypes;
using BlueprintDeck.Node.Registration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlueprintDeck.DependencyInjection
{
    public class ServiceCollectionExtensionsTests
    {

        [Fact]
        public void TestDataTypeRegistration()
        {
            var services = new ServiceCollection();
            services.AddBlueprintDeck(config =>
            {
                config.RegisterDataType<TestType>();
                config.RegisterDataType(typeof(TestType2));
            });
            var provider = services.BuildServiceProvider();

            var registrations = provider.GetServices<DataTypeRegistration>().ToList();
            Assert.NotEmpty(registrations);
            var testRegistration = registrations.FirstOrDefault(x=>x.DataType == typeof(TestType));
            Assert.NotNull(testRegistration);
        }
        
        [Fact]
        public void TestDataTypeRegistration_WhenAlreadyRegisteredType_ThrowsException()
        {
            var services = new ServiceCollection();
            Assert.Throws<Exception>(() =>
            {
                services.AddBlueprintDeck(config =>
                {
                    config.RegisterDataType<TestType>();
                    config.RegisterDataType<TestType>();
                });
            });
        }

        [Fact]
        public void TestDefaultRegistrations()
        {
            var services = new ServiceCollection();
            services.AddBlueprintDeck();
            var provider = services.BuildServiceProvider();
            provider.GetRequiredService<IValueSerializerRepository>();
            provider.GetRequiredService<IBlueprintDeckRegistryFactory>();
            provider.GetRequiredService<IBlueprintFactory>();
            provider.GetRequiredService<INodeFactory>();
            provider.GetRequiredService<IPortConnectionManager>();
        }

        [Fact]
        public void TestNodeRegistration()
        {
            var services = new ServiceCollection();
            services.AddBlueprintDeck(config =>
            {
                config.RegisterNode(typeof(TestableNode<>));
                config.RegisterNode<DoubleNode>();
            });
            var provider = services.BuildServiceProvider();

            var registrations = provider.GetServices<NodeRegistration>().ToList();
            Assert.NotEmpty(registrations);
            var testRegistration = registrations.FirstOrDefault(x=>x.NodeType == typeof(TestableNode<>));
            Assert.NotNull(testRegistration);
            testRegistration = registrations.FirstOrDefault(x=>x.NodeType == typeof(DoubleNode));
            Assert.NotNull(testRegistration);
        }

        private class TestType { }
        private class TestType2 { }
        
    }
}