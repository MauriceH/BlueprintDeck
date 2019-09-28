using System;

namespace BlueprintDeck.Node.Ports.Definitions.DataTypes
{
    public class PdtDuration : IPortData
    {
        public PdtDuration(TimeSpan timeSpan)
        {
            TimeSpan = timeSpan;
        }

        public PdtDuration(string serializedValue)
        {
            if(!long.TryParse(serializedValue,out var duration)) throw new InvalidCastException("Cannot deserialize duration value");
            TimeSpan = TimeSpan.FromMilliseconds(duration);
        }

        public TimeSpan TimeSpan { get; }
        
        public string Serialize()
        {
            return TimeSpan.Milliseconds.ToString();
        }
    }
}