using System;
using BlueprintDeck.Node.Ports.Registration;

namespace BlueprintDeck.Node.Ports
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class PortAttribute : Attribute
    {
        public abstract void Setup(PortRegistration definition);
    }
}