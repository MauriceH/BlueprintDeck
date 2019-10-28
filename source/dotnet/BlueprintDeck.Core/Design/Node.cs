using System.Drawing;

namespace BlueprintDeck.Design
{
    public class Node
    {
        public Point Location { get; set; }
        public string? Title { get; set; }
        public string? Key { get; set; }
        
        public string? NodeTypeKey { get; set; }
        
    }
}