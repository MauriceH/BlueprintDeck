using System;

namespace BlueprintDeck
{
    public class InvalidBluePrintException : Exception
    {
        public InvalidBluePrintException(string message) : base(message)
        {
        }
    }
}