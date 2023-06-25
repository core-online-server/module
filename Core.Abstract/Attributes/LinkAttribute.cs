namespace Core.Abstract.Attributes;

public class LinkAttribute : Attribute
{
    public string Link { get; }

    public LinkAttribute(string link)
    {
        Link = link;
    }
}