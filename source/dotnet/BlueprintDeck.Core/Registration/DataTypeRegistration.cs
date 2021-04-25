using System;

namespace BlueprintDeck.Registration
{
    internal class DataTypeRegistration
    {
        public DataTypeRegistration(string key, Type dataType, string title)
        {
            Key = key;
            DataType = dataType;
            Title = title;
        }

        public string Key { get; }
        public Type DataType { get; }
        public string Title { get; }
    }
}