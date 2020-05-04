using System;

namespace BlueprintDeck.ConstantValue.Serializer
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TimeSpanConstantValueSerializer : IConstantValueSerializer<TimeSpan>
    {
        public string? Serialize(object? value)
        {
            return value switch
            {
                null => null,
                TimeSpan ts => ((long) ts.TotalMilliseconds).ToString(),
                _ => throw new Exception("Cannot convert timespan to long")
            };
        }

        public object? Deserialize(string? serializedValue)
        {
            if (serializedValue == null) return null;
            if (long.TryParse(serializedValue, out var result))
            {
                return TimeSpan.FromMilliseconds(result);
            }
            throw new Exception($"Cannot parse \"{result}\" to TimeSpan");
        }
    }
}