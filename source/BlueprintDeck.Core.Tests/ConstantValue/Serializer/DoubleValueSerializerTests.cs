using System;
using BlueprintDeck.ValueSerializer.Serializer;
using Xunit;

namespace BlueprintDeck.ConstantValue.Serializer
{
    public class DoubleValueSerializerTests
    {
        [Fact]
        public void TestSerialization_WhenDoubleValue_ConvertedCorrect()
        {
            var sut = new DoubleValueSerializer();
            var expected = 22.50000000004d;
            var serializedValue = sut.Serialize(expected);
            var actual = sut.Deserialize(serializedValue);
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void TestSerialization_WhenNullValue_SerializesToNull()
        {
            var sut = new DoubleValueSerializer();
            var serializedValue = sut.Serialize(null);
            Assert.Null(serializedValue);
        }
        [Fact]
        public void TestSerialization_WhenInvalidValue_ThrowsException()
        {
            var sut = new DoubleValueSerializer();
            Assert.Throws<ArgumentException>(() => sut.Serialize("t"));
        }
        [Fact]
        public void TestDeSerialization_WhenNullValue_DeSerializesToNull()
        {
            var sut = new DoubleValueSerializer();
            var deserializedValue = sut.Deserialize(null);
            Assert.Null(deserializedValue);
        }
        [Fact]
        public void TestDeSerialization_WhenInvalidValue_ThrowsException()
        {
            var sut = new DoubleValueSerializer();
            Assert.Throws<ArgumentException>(() => sut.Deserialize("t"));
        }
    }
}