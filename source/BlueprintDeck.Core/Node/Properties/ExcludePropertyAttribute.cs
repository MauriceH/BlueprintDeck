using System;

namespace BlueprintDeck.Node.Properties
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcludePropertyAttribute : Attribute
    {
        
    }
    
    [AttributeUsage(AttributeTargets.Property)]
    public class IncludePropertyAttribute : Attribute
    {
        
    }
    
    [AttributeUsage(AttributeTargets.Class)]
    public class ExcludePropertiesAttribute : Attribute
    {
        
    }
}