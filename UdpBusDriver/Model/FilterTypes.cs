using System.Collections.Generic;
using UdpBus.Model;

namespace UdpBusDriver.Model;

public class FilterTypes
{
    public static KeyValuePair<FilterType, string>[] KnownFilters { get; } =
    {
        new(FilterType.Allow, "无过滤"),
        new(FilterType.Osc, "仅 OSC 数据报"),
        new(FilterType.Deny, "不允许出站")
    };
}
