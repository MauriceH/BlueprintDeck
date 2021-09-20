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
        public NodeInstance(string lifeTimeId, Design.Node nodeDesign, INode node, NodeRegistration registration, List<GenericTypeParameterInstance> genericTypeParameters)
        {
            Registration = registration;
            GenericTypeParameters = genericTypeParameters;
            Node = node;
            Design = nodeDesign;
            LifeTimeId = lifeTimeId;
            Ports = new List<PortInstance>();
        }

        public NodeRegistration Registration { get; }
        public Design.Node Design { get; }
        public INode Node { get; }
        public List<GenericTypeParameterInstance> GenericTypeParameters { get; }

        public bool AllRequiredInputsConnected => Ports
            .Where(x => x.Registration.Direction == Direction.Input
                        && x.Registration.Mandatory)
            .All(x => x.InputOutput != null);

        public List<PortInstance> Ports { get; }
        public string LifeTimeId { get; }

        

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