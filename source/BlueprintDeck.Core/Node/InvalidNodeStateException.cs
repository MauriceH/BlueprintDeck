using System;

namespace BlueprintDeck.Node
{
    public class InvalidNodeStateException : Exception
    {
        public InvalidNodeStateException(string message) : base(message)
        {
        }
    }
}