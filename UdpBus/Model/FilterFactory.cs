using System.Reflection;

namespace UdpBus.Model;

public static class FilterFactory
{
    private static readonly Dictionary<FilterType, Type> Mapping;

    static FilterFactory()
    {
        var fields = Enum.GetNames(typeof(FilterType)).Select(n => typeof(FilterType).GetField(n));
        Mapping = fields.ToDictionary(f => (FilterType) f!.GetRawConstantValue()!,
            f => f!.GetCustomAttribute<FilterClassAttribute>()!.Type);
    }

    public static T GetFilterInstance<T>(this FilterType type)
    {
        return (T) Activator.CreateInstance(Mapping[type])!;
    }
}
