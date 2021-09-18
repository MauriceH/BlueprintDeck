using System;
using System.Collections.Generic;
using System.Reflection;

namespace BlueprintDeck.Misc
{
    internal interface IAssemblyTypesResolver
    {
        IEnumerable<Type> ResolveTypes(Assembly assembly);
    }
}