using System.Threading.Tasks;

namespace BlueprintDeck.Node
{
    public interface INode
    {
        Task Activate(INodeContext nodeContext);
        Task Deactivate();
    }
}