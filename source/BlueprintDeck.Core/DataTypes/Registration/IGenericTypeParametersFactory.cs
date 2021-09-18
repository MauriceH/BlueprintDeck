using System;
using System.Collections.Generic;

namespace BlueprintDeck.DataTypes.Registration
{
    internal interface IGenericTypeParametersFactory
    {
        List<string> CreateGenericTypeList(Type nodeType);
    }
}