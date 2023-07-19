using System.Collections.Generic;
using UdpBus.Model;

namespace UdpBusDriver.Model;

public class FilterTypes
{
    public static KeyValuePair<FilterType, string>[] KnownFilters { get; } =
    {
        new(FilterType.Allow, "TypeAllow"),
        new(FilterType.Osc, "TypeOsc"),
        new(FilterType.Deny, "TypeDeny")
    };
}
