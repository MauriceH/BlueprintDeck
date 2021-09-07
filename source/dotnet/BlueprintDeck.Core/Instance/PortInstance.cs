using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using BlueprintDeck.Node.Ports;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Instance
{
    internal class PortInstance
    {
        public PortInstance(NodePortDefinition definition)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
        }

        public NodePortDefinition Definition { get; }

        public IPortInputOutput? InputOutput { get; set; }


        public void InitializeAsOutput()
        {
            if (Definition.DataMode == DataMode.Simple)
            {
                InputOutput = new SimpleOutput();
                return;
            }

            var d1 = typeof(DataOutput<>);
            Type[] typeArgs = {Definition.PortDataType!};
            var outputType = d1.MakeGenericType(typeArgs);
            InputOutput = (IPortInputOutput)Activator.CreateInstance(outputType);
        }

        public void InitializeAsInput(object connectedOutput)
        {
            if (connectedOutput is SimpleOutput output)
            {
                if (Definition.DataMode == DataMode.Simple)
                {
                    InputOutput = new SimpleInput(output.Observable);
                    return;
                }
            }

            var isDataOutput = connectedOutput.GetType().GetInterfaces()
                .Where(i => i.IsGenericType)
                .Select(i => i.GetGenericTypeDefinition())
                .Contains(typeof(IOutput<>));

            if (isDataOutput)
            {
                var d1 = typeof(DataInput<>);
                Type[] typeArgs = {Definition.PortDataType!};
                var inputType = d1.MakeGenericType(typeArgs);
                var propertyInfo = connectedOutput.GetType().GetProperty("Observable");
                if (propertyInfo == null) throw new Exception("Invalid Observable state");
                var observable = propertyInfo.GetValue(connectedOutput);
                InputOutput = (IPortInputOutput)Activator.CreateInstance(inputType, new[] {observable});
                return;
            }

            throw new Exception("Invalid port connection");
        }

        public override string ToString()
        {
            return $"Key {Definition.Key} Type {Definition.InputOutputType} Mode {Definition.DataMode} DataType {Definition.PortDataType}";
        }

        public void InitializeAsInputToConstantValue(ConstantValueInstance constantValue)
        {
            var d1 = typeof(DataInput<>);
            Type[] typeArgs = {constantValue.DataType};
            var inputType = d1.MakeGenericType(typeArgs);
            var methodInfo = typeof(Observable).GetMethod("OfType", BindingFlags.Public | BindingFlags.Static);
            if (methodInfo == null) throw new Exception("Internal Method not valid");
            var method = methodInfo.MakeGenericMethod(constantValue.DataType);
            if (method == null) throw new Exception("Invalid Observable state");
            var observable = method.Invoke(null, new object[] {constantValue.Observable});
            InputOutput = (IPortInputOutput)Activator.CreateInstance(inputType, observable);
        }
    }
}