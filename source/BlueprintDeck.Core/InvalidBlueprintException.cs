using System;

namespace BlueprintDeck
{
    public class InvalidBlueprintException : Exception
    {
        public InvalidBlueprintException(string message) : base(message)
        {
        }
    }
}