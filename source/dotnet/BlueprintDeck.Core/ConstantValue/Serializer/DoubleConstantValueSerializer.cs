using System;

namespace BlueprintDeck.ConstantValue.Serializer
{
    public class DoubleConstantValueSerializer : IConstantValueSerializer
    {
        public string Serialize(object value)
        {
            return $"{value}";
        }

        public Type GetDataType()
        {
            return typeof(double);
        }

        public object Deserialize(string serializedValue)
        {
            if (!double.TryParse(serializedValue, out var dbl)) throw new ArgumentException("Value cannot be parsed to double");
            return dbl;
        }
    }
}