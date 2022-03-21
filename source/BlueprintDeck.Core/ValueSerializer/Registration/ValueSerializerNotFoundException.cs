using System;

namespace BlueprintDeck.ValueSerializer.Registration
{
    public class ValueSerializerNotFoundException : Exception
    {
        public ValueSerializerNotFoundException(Type type) : base($"No constant value serializer found for type \"{type.Name}\"")
        {
        }
    }
}