namespace Core.Abstract.Attributes;

public class ElementAttribute : Attribute
{
    public string Variant { get; }

    public string Subject { get; }

    public ElementAttribute(string variant, string subject)
    {
        Variant = variant;
        Subject = subject;
    }

    public ElementAttribute(string subject)
    {
        Variant = string.Empty;
        Subject = subject;
    }
}