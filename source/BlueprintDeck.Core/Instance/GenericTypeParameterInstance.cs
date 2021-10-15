using System;

namespace BlueprintDeck.Instance
{
    internal class GenericTypeParameterInstance
    {
        
        public string Key { get;  }
        public Type DataType { get; }

        public GenericTypeParameterInstance(string key, Type dataType)
        {
            Key = key;
            DataType = dataType;
        }
    }
}