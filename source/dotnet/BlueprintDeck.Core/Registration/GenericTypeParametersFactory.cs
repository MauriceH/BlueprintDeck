using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BlueprintDeck.Registration
{
    public class GenericTypeParametersFactory
    {
        public static List<string> CreateGenericTypeList(Type nodeType)
        {
            if (!nodeType.IsGenericType) return new List<string>();
            return nodeType.GetTypeInfo().GenericTypeParameters.Select(x => x.Name).ToList();
        }
    }
}