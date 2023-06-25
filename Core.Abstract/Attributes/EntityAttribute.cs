namespace Core.Abstract.Attributes;

public class EntityAttribute : Attribute
{
    public string Variant { get; }

    public string Subject { get; }

    public EntityAttribute(string variant, string subject)
    {
        Variant = variant;
        Subject = subject;
    }

    public EntityAttribute(string subject)
    {
        Variant = string.Empty;
        Subject = subject;
    }
}