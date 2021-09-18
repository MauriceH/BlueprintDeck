using System;
using System.Collections.Generic;
using System.Linq;
using BlueprintDeck.Node;
using BlueprintDeck.Node.Ports;
using BlueprintDeck.Node.Ports.Registration;
using BlueprintDeck.Node.Registration;

namespace BlueprintDeck.Instance
{
    internal class NodeInstance
    {
        public NodeInstance(string lifeTimeId, Design.Node nodeDesign, INode node, NodeRegistration registration)
        {
            Registration = registration;
            Node = node;
            Design = nodeDesign;
            LifeTimeId = lifeTimeId;
            Ports = new List<PortInstance>();
        }

        public NodeRegistration Registration { get; }
        public Design.Node Design { get; }
        public INode Node { get; }

        public bool AllRequiredInputsConnected => Ports
            .Where(x => x.Registration.Direction == Direction.Input
                        && x.Registration.Mandatory)
            .All(x => x.InputOutput != null);

        public List<PortInstance> Ports { get; }
        public string LifeTimeId { get; }

        public T? GetPort<T>(PortRegistration definition) where T: class, IPortInputOutput 
        {
            var port = Ports.FirstOrDefault(x => x.Registration.Key == definition.Key);
            if (port == null) throw new Exception("Port not found");
            if (port.Registration.Mandatory)
            {
                if (port.InputOutput == null) throw new Exception("Port not connected");    
            }
            return (T?)port?.InputOutput;
        }

        public void Activate()
        {
            Node.Activate();
        }

        public override string ToString()
        {
            return $"Type {Registration.Id}";
        }
    }
}