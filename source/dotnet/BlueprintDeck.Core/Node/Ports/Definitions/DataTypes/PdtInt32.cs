using System;

namespace BlueprintDeck.Node.Ports.Definitions.DataTypes
{
    public class PdtInt32 : IPortData
    {
        public PdtInt32(int value)
        {
            Value = value;
        }

        public PdtInt32(string serializedValue)
        {
            if (!int.TryParse(serializedValue, out var val)) throw new InvalidCastException("Cannot deserialize duration value");
            Value = val;
        }

        public int Value { get; }

        public string Serialize()
        {
            return Value.ToString();
        }
    }
}