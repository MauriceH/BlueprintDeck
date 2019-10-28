using System;

namespace BlueprintDeck.ConstantValue.Serializer
{
    public interface IConstantValueSerializer
    {
        string Serialize(object value);
        Type GetDataType();
        object Deserialize(string serializedValue);

    }
    
}