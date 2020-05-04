using System;
using System.Collections.Generic;
using System.Linq;
using BlueprintDeck.Node;
using BlueprintDeck.Node.Ports;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Instance
{
    public class NodeInstance : INodeContext
    {
        public NodeInstance(string lifeTimeId, Design.Node nodeDesign, INode node, NodeDescriptorAttribute descriptor)
        {
            Descriptor = descriptor;
            Node = node;
            Design = nodeDesign;
            LifeTimeId = lifeTimeId;
            Ports = new List<PortInstance>();
        }

        public NodeDescriptorAttribute Descriptor { get; }
        public Design.Node Design { get; }
        public INode Node { get; }

        public bool AllRequiredInputsConnected => Ports
            .Where(x => x.Definition.InputOutputType == InputOutputType.Input
                        && x.Definition.Mandatory)
            .All(x => x.InputOutput != null);

        public List<PortInstance> Ports { get; }
        public string LifeTimeId { get; }

        public T? GetPort<T>(NodePortDefinition definition) where T: class, IPortInputOutput 
        {
            var port = Ports.FirstOrDefault(x => x.Definition.Key == definition.Key);
            if (port == null) throw new Exception("Port not found");
            if (port.InputOutput == null) throw new Exception("Port not found");
            return (T?)port?.InputOutput;
        }

        public void Activate()
        {
            Node.Activate(this);
        }

        public override string ToString()
        {
            return $"Type {Descriptor.Id}";
        }
    }
}