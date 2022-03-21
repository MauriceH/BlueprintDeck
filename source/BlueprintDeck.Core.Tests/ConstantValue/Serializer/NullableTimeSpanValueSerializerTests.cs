using System;
using BlueprintDeck.ValueSerializer.Serializer;
using Xunit;

namespace BlueprintDeck.ConstantValue.Serializer
{
    public class NullableTimeSpanValueSerializerTests
    {
        private readonly NullableTimeSpanValueSerializer _sut = new ();

        [Fact]
        public void TestSerialization_WhenTimeSpanValue_ConvertedCorrect()
        {
            var expected = TimeSpan.FromMilliseconds(3660);
            var serializedValue = _sut.Serialize(expected);
            var actual = _sut.Deserialize(serializedValue);
            Assert.Equal(expected, actual);
        }
       
        [Fact]
        public void TestSerialization_WhenNullValue_SerializesToNull()
        {
            var serializedValue = _sut.Serialize(null);
            Assert.Null(serializedValue);
        }
        [Fact]
        public void TestSerialization_WhenInvalidValue_ThrowsException()
        {
            Assert.Throws<Exception>(() => _sut.Serialize("t"));
        }
        [Fact]
        public void TestDeSerialization_WhenNullValue_DeSerializesToNull()
        {
            var deserializedValue = _sut.Deserialize(null);
            Assert.Null(deserializedValue);
        }
        [Fact]
        public void TestDeSerialization_WhenInvalidValue_ThrowsException()
        {
            Assert.Throws<Exception>(() => _sut.Deserialize("t"));
        }
        [Fact]
        public void TestDeSerialization_WhenNullValue_ReturnsNull()
        {
            Assert.Null(_sut.Serialize(null));
            Assert.Null(_sut.Deserialize(null));
        }
    }
}