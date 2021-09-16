using System;
using BlueprintDeck.ConstantValue.Serializer;
using Xunit;

namespace BluePrintDeck.ConstantValue.Serializer
{
    public class Int32ConstantValueSerializerTests
    {
        private readonly Int32ConstantValueSerializer _sut = new ();

        [Fact]
        public void TestSerialization_WhenInt32MaxValue_ConvertedCorrect()
        {
            var expected = int.MaxValue;
            var serializedValue = _sut.Serialize(expected);
            var actual = _sut.Deserialize(serializedValue);
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void TestSerialization_WhenInt32MinValue_ConvertedCorrect()
        {
            var expected = int.MinValue;
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
            Assert.Throws<ArgumentException>(() => _sut.Serialize("t"));
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
            Assert.Throws<ArgumentException>(() => _sut.Deserialize("t"));
        }
    }
}