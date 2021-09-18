using System.Diagnostics.CodeAnalysis;

namespace BlueprintDeck.Design
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class NodeLocation
    {
        public int X { get; set; }
        public int Y { get; set; }

        public NodeLocation()
        {
        }

        public NodeLocation(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}