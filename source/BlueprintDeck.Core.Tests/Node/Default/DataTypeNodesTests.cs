using System;
using System.Threading.Tasks;
using BlueprintDeck.Node.Default.DataTypes;
using BlueprintDeck.Node.Ports;
using Xunit;

namespace BlueprintDeck.Node.Default
{
    public class DataTypeNodesTests
    {

        [Fact]
        public async Task TestDoubleNode()
        {
            await TestValueNode<DoubleNode, double>(123.4d);
            await TestValueNode<DecimalNode, decimal>(123.4m);
            await TestValueNode<FloatNode, float>(123.4f);
            await TestValueNode<Int32Node, int>(123);
            await TestValueNode<Int64Node, long>(123L);
            await TestValueNode<TimeSpanNode, TimeSpan>(TimeSpan.FromSeconds(123));
            await TestValueNode<DateTimeNode, DateTime>(DateTime.Now);
        }

        private async Task TestValueNode<T, TType>(TType propertyValue) where T : ConstantValueNode<TType>, new()
        {
            var node = new T();
            var tempValue = default(TType);
            var output = new DataOutput<TType>();
            output.Observable.Subscribe(value => tempValue = value);
            node.Value = propertyValue;
            node.Output = output;
            await node.Activate();
            await node.Deactivate();
            Assert.Equal(propertyValue,tempValue);
        }

    }
}