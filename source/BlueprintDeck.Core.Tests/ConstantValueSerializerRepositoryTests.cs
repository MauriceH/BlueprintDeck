using System.Diagnostics.CodeAnalysis;
using BlueprintDeck.ConstantValue.Serializer;
using BlueprintDeck.ValueSerializer;
using BlueprintDeck.ValueSerializer.Registration;
using BlueprintDeck.ValueSerializer.Serializer;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlueprintDeck
{
    [ExcludeFromCodeCoverage]
    public class ConstantValueSerializerRepositoryTests
    {
        private readonly IValueSerializerRepository _sut;
        private readonly DoubleValueSerializer _doubleSerializer = new();
        
        public ConstantValueSerializerRepositoryTests()
        {
            var collection = new ServiceCollection();
            collection.AddSingleton<IValueSerializer<double>>(_doubleSerializer);
            var provider = collection.BuildServiceProvider();
            _sut = new ValueSerializerRepository(provider);
        }

        [Fact]
        public void TestLoadSerializer_WhenTypeDouble_ResolveDoubleConstantValueSerializer()
        {
            var actual = _sut.LoadSerializer(typeof(double));
            Assert.Same(_doubleSerializer,actual);
            //Just for coverage, second run of registry cache
            actual = _sut.LoadSerializer(typeof(double));
            Assert.Same(_doubleSerializer,actual);
        }
        
        [Fact]
        public void TestLoadSerializer_WhenTypeInteger_ThrowsException()
        {
            Assert.Throws<ValueSerializerNotFoundException>(() =>
            {
                _sut.LoadSerializer(typeof(int));
            });
        }
        
    }
}