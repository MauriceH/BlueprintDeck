using System.Threading.Tasks;

namespace BlueprintDeck.Node
{
    public interface INode<T>
    {
        Task Activate(INodeContext nodeContext, T ports);
        Task Deactivate();
    }
    
    public interface INode
    {
        Task Activate(INodeContext nodeContext);
        Task Deactivate();
    }
}