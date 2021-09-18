using System;
using System.Reflection;

namespace BlueprintDeck.Node.Properties.Registration
{
    public class PropertyRegistration
    {
        internal PropertyInfo PropertyInfo { get; }

        public string Name => PropertyInfo.Name;

        public Type Type => PropertyInfo.PropertyType;
        
        public string? Title { get; set; }

        internal PropertyRegistration(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }
       
    }
}