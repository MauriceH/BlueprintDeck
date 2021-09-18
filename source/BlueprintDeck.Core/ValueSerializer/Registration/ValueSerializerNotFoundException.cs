using System;

namespace BlueprintDeck.ConstantValue.Serializer
{
    public class ValueSerializerNotFoundException : Exception
    {
        public ValueSerializerNotFoundException(Type type) : base($"No constant value serializer found for type \"{type.Name}\"")
        {
        }
    }
}