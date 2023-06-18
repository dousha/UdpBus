namespace UdpBus.Model;

[AttributeUsage(AttributeTargets.Field)]
public class FilterClassAttribute : Attribute
{
    public FilterClassAttribute(Type type)
    {
        Type = type;
    }

    public Type Type { get; }
}
