using System.Drawing;

namespace BlueprintDeck.Design
{
    public class NodeDesignInstance
    {
        public Point Location { get; set; }
        
        public string? NodeTypeKey { get; set; }
        
        public string? NodeInstanceId { get; set; }
        
        public string? Title { get; set; }
        
//        public List<DesignPortInstance>? Ports { get; set; }
        
        public string? Value { get; set; }
        
    }
}