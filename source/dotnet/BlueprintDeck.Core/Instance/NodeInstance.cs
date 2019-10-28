using System;
using System.Collections.Generic;
using System.Linq;
using BlueprintDeck.Design;
using BlueprintDeck.Node;
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

        public bool AllRequiredInputsConnected => Ports.Where(x => x.Definition.InputOutputType == InputOutputType.Input && x.Definition.Mandatory).All(x => x.InputOutput != null);
        public List<PortInstance> Ports { get; }
        public string LifeTimeId { get; }

      
        public void SetValue(string constantValue)
        {
            var type = Node.GetType();
            Type valueType;
            if (type.IsGenericType)
            {
                valueType = type.GetGenericArguments()[0];
            }
            else
            {
                if (type.BaseType?.IsGenericType ?? false)
                {
                    valueType = type.BaseType.GetGenericArguments()[0];
                }
                else
                {
                    throw new Exception("Invalid Type");
                }
            }
            
            
            var valueInstance = Activator.CreateInstance(valueType, constantValue);
            type.GetMethod("ChangeValue")?.Invoke(Node, new []{valueInstance});
        }

        public T GetPort<T>(NodePortDefinition definition)
        {
            var port = Ports.FirstOrDefault(x => x.Definition.Key == definition.Key);
            if (port == null) throw new Exception("Port not found");
            return (T) port?.InputOutput;
        }

        public void Activate()
        {
            Node.Activate(this);
        }


        public bool CheckIfValid()
        {
            foreach (var port in Ports)
            {
                if (port.Definition.Mandatory)
                {
                    if (port.InputOutput == null) return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            return $"Type {Descriptor.Id}";
        }
    }
}