using System;
using System.Collections.Generic;
using System.Reflection;
using BlueprintDeck.Misc;

namespace BlueprintDeck.Node.Properties.Registration
{
    internal class PropertyRegistrationFactory : IPropertyRegistrationFactory
    {
        public IList<PropertyRegistration> CreatePropertyRegistrations(Type nodeType)
        {
            var propertyInfos = nodeType.GetProperties();
            var result = new List<PropertyRegistration>();
            var includeAllProperties = nodeType.GetCustomAttribute<ExcludePropertiesAttribute>() == null;

            foreach (var property in propertyInfos)
            {
                var propertyType = property.PropertyType;

                if (propertyType.IsInput() || propertyType.IsOutput()) continue;
                if (propertyType == typeof(Design.Node)) continue;
                if (property.GetCustomAttribute<ExcludePropertyAttribute>() != null) continue;
                if (property.GetCustomAttribute<IncludePropertyAttribute>() == null)
                {
                    if(!includeAllProperties) continue;    
                }
                
                var registration = new PropertyRegistration(property);

                var customAttributes = property.GetCustomAttributes(true);
                foreach (var attribute in customAttributes)
                {
                    if (attribute is not PropertyAttribute pa) continue;
                    pa.Setup(registration);
                }

                result.Add(registration);
            }

            return result;
        }
    }
}