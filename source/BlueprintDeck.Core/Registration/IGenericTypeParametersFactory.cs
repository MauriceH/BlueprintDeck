using System;
using System.Collections.Generic;

namespace BlueprintDeck.Registration
{
    internal interface IGenericTypeParametersFactory
    {
        List<string> CreateGenericTypeList(Type nodeType);
    }
}