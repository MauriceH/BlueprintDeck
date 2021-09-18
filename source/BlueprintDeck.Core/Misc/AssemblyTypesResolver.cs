using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BlueprintDeck.Misc
{
    internal class AssemblyTypesResolver : IAssemblyTypesResolver
    {
        public IEnumerable<Type> ResolveTypes(Assembly assembly)
        {
            Type[] types;
            try
            {
                types = assembly.GetTypes().ToArray();
            }
            catch (ReflectionTypeLoadException e)
            {
                types = e.Types.Where(x=>x != null).Cast<Type>().ToArray();
            }
            return types;
        }
    }
}