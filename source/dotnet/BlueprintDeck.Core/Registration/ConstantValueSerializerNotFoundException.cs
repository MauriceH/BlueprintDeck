using System;

namespace BlueprintDeck.Registration
{
    public class ConstantValueSerializerNotFoundException : Exception
    {
        public ConstantValueSerializerNotFoundException(Type type) : base($"No constant value serializer found for type \"{type.Name}\"")
        {
        }
    }
}