using System;

namespace BlueprintDeck.ConstantValue.Serializer
{
    public class StringConstantValueSerializer : IConstantValueSerializer
    {
        public string Serialize(object value)
        {
            return $"{value}";
        }

        public Type GetDataType()
        {
            return typeof(string);
        }

        public object Deserialize(string serializedValue)
        {
            return serializedValue;
        }
    }
}