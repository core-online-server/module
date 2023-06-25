namespace Core.Abstract.Attributes;

public class PriorityAttribute : Attribute
{
    public double Priority { get; }

    public PriorityAttribute(double priority)
    {
        Priority = priority;
    }
}