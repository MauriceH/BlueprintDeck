using System;

namespace BlueprintDeck.Registration
{
    public class SerializerNotFoundException : Exception
    {
        public SerializerNotFoundException(string message) : base(message)
        {
        }
    }
}