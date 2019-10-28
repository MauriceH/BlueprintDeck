using System;

namespace BlueprintDeck.ConstantValue.Serializer
{
    public class Int32ConstantValueSerializer : IConstantValueSerializer
    {
        public string Serialize(object value)
        {
            return value.ToString();
        }
        
        public Type GetDataType()
        {
            return typeof(int);
        }

        public object Deserialize(string serializedValue)
        {
            if (int.TryParse(serializedValue, out var result)) return result;
            throw new Exception($"Cannot parse \"{result}\" to int32");
        }
    }
}