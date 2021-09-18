using System;
using Xunit;

namespace BlueprintDeck.ConstantValue.Serializer
{
    public class StringConstantValueSerializerTests
    {
        private readonly StringConstantValueSerializer _sut = new ();

        [Fact]
        public void TestSerialization_WhenString_ConvertedCorrect()
        {
            var expected = "Test";
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
            Assert.Throws<ArgumentException>(() => _sut.Serialize(765));
        }
        [Fact]
        public void TestDeSerialization_WhenNullValue_DeSerializesToNull()
        {
            var deserializedValue = _sut.Deserialize(null);
            Assert.Null(deserializedValue);
        }
    }
}