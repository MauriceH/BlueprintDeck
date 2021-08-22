using System.Threading.Tasks;

namespace BlueprintDeck.Node
{
    public interface INode
    {
        Design.Node DesignValues { get; set; }
        Task Activate(INodeContext nodeContext);
        Task Deactivate();
    }
}