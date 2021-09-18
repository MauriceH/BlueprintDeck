using System;

namespace BlueprintDeck.Misc
{
    public class InvalidBlueprintException : Exception
    {
        public InvalidBlueprintException(string message) : base(message)
        {
        }
    }
}