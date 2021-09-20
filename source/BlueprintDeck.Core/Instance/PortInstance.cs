using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
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


        

        public override string ToString()
        {
            return $"Key {Registration.Key} Type {Registration.Direction} DataType {Registration.DataType}";
        }

        // public void InitializeAsInputToConstantValue(ConstantValueInstance constantValue)
        // {
        //     var d1 = typeof(DataInput<>);
        //     Type[] typeArgs = {constantValue.DataType};
        //     var inputType = d1.MakeGenericType(typeArgs);
        //     var methodInfo = typeof(Observable).GetMethod("OfType", BindingFlags.Public | BindingFlags.Static);
        //     if (methodInfo == null) throw new Exception("Internal Method not valid");
        //     var method = methodInfo.MakeGenericMethod(constantValue.DataType);
        //     if (method == null) throw new Exception("Invalid Observable state");
        //     var observable = method.Invoke(null, new object[] {constantValue.Observable});
        //     InputOutput = (IPort?)Activator.CreateInstance(inputType, observable);
        // }
    }
}