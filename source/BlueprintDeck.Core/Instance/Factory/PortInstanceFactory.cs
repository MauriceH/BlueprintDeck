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

            if (portRegistration.GenericTypeParameter != null)
            {
                if (!nodeInstance.GenericTypeParameters.Any())
                    throw new Exception(
                        $"Invalid node instance {nodeInstance.Registration.Id}. Port {portInstance.Registration.Key} generic type {portInstance.Registration.GenericTypeParameter} not found in node instance");

                var generic = nodeInstance.GenericTypeParameters.FirstOrDefault(x => x.Key == portRegistration.GenericTypeParameter);
                if (generic == null)
                    throw new Exception(
                        $"Invalid node instance {nodeInstance.Design.Id}. Generic port {portInstance.Registration.Key} type {portInstance.Registration.GenericTypeParameter} not found");

                portDataType = generic.DataType;

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

            Type portDataType;

            if (portInstance.Registration.IsGeneric)
            {
                var genericTypeKey = portInstance.Registration.GenericTypeParameter;
                var genericType = nodeInstance.GenericTypeParameters.FirstOrDefault(x => x.Key == genericTypeKey);
                if(genericType == null) throw new Exception($"Missing generic type for port {portInstance.Registration.Key} on node {nodeInstance.Registration.Id}");

                portDataType = genericType.DataType;

            }
            else
            {
                if (portInstance.Registration.DataType == null) throw new Exception($"Node with id {nodeInstance.Registration.Id} has an invalid definition for port {portInstance.Registration.Key}");
                portDataType = portInstance.Registration.DataType!;
            }
            
            var connectedIsDataOutput = connectedOutput.GetType().GetInterfaces()
                .Where(i => i.IsGenericType)
                .SelectMany(i => i.GetGenericArguments())
                .Contains(portDataType);

            if (!connectedIsDataOutput)
            {
                throw new Exception($"Data type conflict for Node with id {nodeInstance.Registration.Id} and port {portInstance.Registration.Key}");
            }
           

            var genericInputType = typeof(DataInput<>);
            Type[] typeArgs = { portDataType };
            var inputType = genericInputType.MakeGenericType(typeArgs);
            var propertyInfo = connectedOutput.GetType().GetProperty(nameof(DataOutput<object>.Observable));
            if (propertyInfo == null) throw new Exception($"Port connected to node with id {nodeInstance.Registration.Id} has an invalid port-instance");
            var observable = propertyInfo.GetValue(connectedOutput);
            portInstance.InputOutput = (IPort?)Activator.CreateInstance(inputType, new[] { observable });
            
        }
    }
}