using System.Threading.Tasks;

namespace BlueprintDeck.Node
{
    public interface INode
    {
        Task Activate();
        Task Deactivate();
    }
}