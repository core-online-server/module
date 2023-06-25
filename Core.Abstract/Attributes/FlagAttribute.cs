namespace Core.Abstract.Attributes;

public class FlagAttribute : Attribute
{
    public string Flag { get; }

    public int Index { get; }

    public FlagAttribute(string link, int index = -1)
    {
        Flag = link;
        Index = index;
    }
}