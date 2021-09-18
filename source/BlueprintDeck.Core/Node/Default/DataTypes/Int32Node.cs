using System;

namespace BlueprintDeck.Node.Default.DataTypes
{
    [Node]
    public class Int32Node : ConstantValueNode<int>
    {
    }
    
    [Node]
    public class Int64Node : ConstantValueNode<long>
    {
    }

    [Node]
    public class DoubleNode : ConstantValueNode<double>
    {
    }

    [Node]
    public class TimeSpanNode : ConstantValueNode<TimeSpan>
    {
    }

    [Node]
    public class FloatNode : ConstantValueNode<float>
    {
    }

    [Node]
    public class DecimalNode : ConstantValueNode<decimal>
    {
    }

    [Node]
    public class DateTimeNode : ConstantValueNode<DateTime>
    {
    }

}