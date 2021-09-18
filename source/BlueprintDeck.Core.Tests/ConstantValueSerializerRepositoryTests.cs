using System.Diagnostics.CodeAnalysis;
using BlueprintDeck.ConstantValue.Registration;
using BlueprintDeck.ConstantValue.Serializer;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlueprintDeck
{
    [ExcludeFromCodeCoverage]
    public class ConstantValueSerializerRepositoryTests
    {
        private readonly IConstantValueSerializerRepository _sut;
        private readonly DoubleConstantValueSerializer _doubleSerializer = new();
        
        public ConstantValueSerializerRepositoryTests()
        {
            var collection = new ServiceCollection();
            collection.AddSingleton<IConstantValueSerializer<double>>(_doubleSerializer);
            var provider = collection.BuildServiceProvider();
            _sut = new ConstantValueSerializerRepository(provider);
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
            Assert.Throws<ConstantValueSerializerNotFoundException>(() =>
            {
                _sut.LoadSerializer(typeof(int));
            });
        }
        
    }
}