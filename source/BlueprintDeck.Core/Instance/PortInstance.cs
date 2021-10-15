using System;
using BlueprintDeck.Node.Ports;
using BlueprintDeck.Node.Ports.Registration;

namespace BlueprintDeck.Instance
{
    internal class PortInstance
    {
        public PortInstance(PortRegistration portRegistration)
        {
            Registration = portRegistration ?? throw new ArgumentNullException(nameof(portRegistration));
        }

        public PortRegistration Registration { get; }

        public IPort? InputOutput { get; set; }

    }
}