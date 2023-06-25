namespace Core.Abstract.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class AlsoAttribute : Attribute
{
    public string Method { get; }

    public string Parameter { get; }

    public int Value { get; }

    public AlsoAttribute(string method, string parameter, int value)
    {
        Method = method;
        Parameter = parameter;
        Value = value;
    }
}