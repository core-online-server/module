namespace Core.Abstract.Attributes;

public class LengthAttribute : Attribute
{
    public int Length { get; }

    public LengthAttribute(int length)
    {
        Length = length;
    }
}