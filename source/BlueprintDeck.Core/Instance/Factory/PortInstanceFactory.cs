using System;
using System.Linq;
using System.Threading;
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


        public void InitializeAsOutput(NodeInstance nodeInstance, PortInstance portInstance)
        {
            var portRegistration = portInstance.Registration;
            if (!portRegistration.WithData)
            {
                portInstance.InputOutput = new SimpleOutput();
                return;
            }

            var portDataType = portRegistration.DataType;

            if (portRegistration.GenericTypeParameterName != null)
            {
                if (!nodeInstance.GenericTypeParameters.Any())
                    throw new Exception(
                        $"Invalid node instance {nodeInstance.Registration.Id}. Port {portInstance.Registration.Key} generic type {portInstance.Registration.GenericTypeParameterName} not found in node instance");

                var generic = nodeInstance.GenericTypeParameters.FirstOrDefault(x => x.Key == portRegistration.GenericTypeParameterName);
                if (generic == null)
                    throw new Exception(
                        $"Invalid node instance {nodeInstance.Design.Id}. Generic port {portInstance.Registration.Key} type {portInstance.Registration.GenericTypeParameterName} not found");
            }

            if (portDataType == null)
                throw new Exception($"Invalid node instance {nodeInstance.Design.Id}. Port {portInstance.Registration.Key} type is empty");
            Type[] typeArgs = { portDataType };
            var outputType = typeof(DataOutput<>).MakeGenericType(typeArgs);
            portInstance.InputOutput = (IPort?)Activator.CreateInstance(outputType);
        }

        public void InitializeAsInput(NodeInstance nodeInstance, PortInstance portInstance, IPort connectedOutput)
        {
            if (!portInstance.Registration.WithData)
            {
                if (connectedOutput is not SimpleOutput output) throw new Exception("invalid port connection");
                portInstance.InputOutput = new SimpleInput(output.Observable);
                return;
            }

            var outputType = typeof(DataOutput<>).MakeGenericType(portInstance.Registration.DataType!);

            var connectedIsDataOutput = connectedOutput.GetType().GetInterfaces()
                .Where(i => i.IsGenericType)
                .Select(i => i.GetGenericTypeDefinition())
                .Contains(outputType);

            if (connectedIsDataOutput)
            {
                var genericInputType = typeof(DataInput<>);
                Type[] typeArgs = { portInstance.Registration.DataType! };
                var inputType = genericInputType.MakeGenericType(typeArgs);

                var propertyInfo = connectedOutput.GetType().GetProperty("Observable");
                if (propertyInfo == null) throw new Exception("Invalid Observable state");
                var observable = propertyInfo.GetValue(connectedOutput);
                portInstance.InputOutput = (IPort?)Activator.CreateInstance(inputType, new[] { observable });
                return;
            }

            throw new Exception("Invalid port connection");
        }
    }
}