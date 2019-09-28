using System.Threading.Tasks;

namespace BlueprintDeck.Node
{
    public interface INode
    {
        string? ShortTitle { get; set; }
        Task Activate(INodeContext nodeContext);
        Task Deactivate();
    }
}