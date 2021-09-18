using System;
using BlueprintDeck.Registration;

namespace BlueprintDeck.Node
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class PropertyAttribute : Attribute
    {
        public abstract void Setup(PropertyDefinition definition);
    }
}