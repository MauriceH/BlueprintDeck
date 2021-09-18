using System;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Node
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class PortAttribute : Attribute
    {
        public abstract void Setup(PortRegistration definition);
    }
}