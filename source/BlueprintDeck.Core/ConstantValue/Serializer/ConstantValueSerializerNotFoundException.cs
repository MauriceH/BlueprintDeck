using System;

namespace BlueprintDeck.ConstantValue.Serializer
{
    public class ConstantValueSerializerNotFoundException : Exception
    {
        public ConstantValueSerializerNotFoundException(Type type) : base($"No constant value serializer found for type \"{type.Name}\"")
        {
        }
    }
}