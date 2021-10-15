using System.Collections.Generic;
using System.Reactive.Subjects;
using BlueprintDeck.Node.Default;
using BlueprintDeck.Node.Default.DataTypes;
using BlueprintDeck.Node.Ports;
using BlueprintDeck.Node.Ports.Registration;
using BlueprintDeck.Node.Properties.Registration;
using BlueprintDeck.Node.Registration;
using Xunit;

namespace BlueprintDeck.Instance
{
    public class NodeInstanceTests
    {
        [Fact]
        public void TestNodeInstance()
        {
            var node = new ToStringNode<string>();
            var registration = new NodeRegistration("id", "title", node.GetType(), new List<PortRegistration>(), new List<string>(),
                new List<PropertyRegistration>());
            var nodeDesign = new Design.Node();
            
            var sut = new NodeInstance("123", nodeDesign, node, registration, new List<GenericTypeParameterInstance>());

            var p1 = new PortInstance(new PortRegistration(node.GetType().GetProperty(nameof(ToStringNode<string>.Input)),Direction.Input));
            var p2= new PortInstance(new PortRegistration(node.GetType().GetProperty(nameof(ToStringNode<string>.Input)),Direction.Input));

            p1.InputOutput = new SimpleInput(new Subject<object>());
            
            sut.Ports.Add(p1);
           
            
            Assert.Equal("123",sut.LifeTimeId);
            Assert.Same(registration,sut.Registration);
            Assert.Same(nodeDesign,sut.Design);
            Assert.NotEmpty(sut.Ports);
            Assert.True(sut.AllRequiredInputsConnected);
            
            sut.Ports.Add(p2);
            Assert.False(sut.AllRequiredInputsConnected);
            
        }
        
    }
}