using System;
using System.Linq;
using BlueprintDeck.Node.Ports;
using BlueprintDeck.Node.Ports.Registration;

namespace BlueprintDeck.Instance.Factory
{
    internal class PortInstanceFactory : IPortInstanceFactory
    {
        public PortInstance Create(PortRegistration portRegistration)
        {
            return new PortInstance(portRegistration);
        }


        public void InitializeAsOutput(PortInstance portInstance)
        {
            var registration = portInstance.Registration;
            if (!registration.WithData)
            {
                portInstance.InputOutput = new SimpleOutput();
                return;
            }

            if (registration.DataType != null)
            {
                var d1 = typeof(DataOutput<>);
                Type[] typeArgs = { registration.DataType! };
                var outputType = d1.MakeGenericType(typeArgs);
                portInstance.InputOutput = (IPortInputOutput?)Activator.CreateInstance(outputType);
                return;
            }

            //TODO Generic typed ports
        }

        public void InitializeAsInput(PortInstance portInstance, object connectedOutput)
        {
            if (connectedOutput is SimpleOutput output)
            {
                if (!portInstance.Registration.WithData)
                {
                    portInstance.InputOutput = new SimpleInput(output.Observable);
                    return;
                }
                throw new Exception("Invalid connection");
            }

            var isDataOutput = connectedOutput.GetType().GetInterfaces()
                .Where(i => i.IsGenericType)
                .Select(i => i.GetGenericTypeDefinition())
                .Contains(typeof(IOutput<>));

            if (isDataOutput)
            {
                var d1 = typeof(DataInput<>);
                Type[] typeArgs = { portInstance.Registration.DataType! };
                var inputType = d1.MakeGenericType(typeArgs);
                var propertyInfo = connectedOutput.GetType().GetProperty("Observable");
                if (propertyInfo == null) throw new Exception("Invalid Observable state");
                var observable = propertyInfo.GetValue(connectedOutput);
                portInstance.InputOutput = (IPortInputOutput?)Activator.CreateInstance(inputType, new[] { observable });
                return;
            }

            throw new Exception("Invalid port connection");
        }
    }
}