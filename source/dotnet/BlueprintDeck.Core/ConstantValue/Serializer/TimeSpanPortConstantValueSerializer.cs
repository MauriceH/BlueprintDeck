using System;

namespace BlueprintDeck.ConstantValue.Serializer
{
    public class TimeSpanConstantValueSerializer : IConstantValueSerializer
    {
        public string Serialize(object value)
        {
            if (value is TimeSpan ts)
            {
                return ((long) ts.TotalMilliseconds).ToString();
            }
            throw new Exception("Cannot convert timespan to long");
        }
        
        public Type GetDataType()
        {
            return typeof(TimeSpan);
        }

        public object Deserialize(string serializedValue)
        {
            if (long.TryParse(serializedValue, out var result))
            {
                return TimeSpan.FromMilliseconds(result);
            }
            throw new Exception($"Cannot parse \"{result}\" to TimeSpan");
        }
    }
}