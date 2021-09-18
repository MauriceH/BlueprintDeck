using System;
using BlueprintDeck.Node.Properties.Registration;

namespace BlueprintDeck.Node.Properties
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class PropertyAttribute : Attribute
    {
        public abstract void Setup(PropertyRegistration definition);
    }
}