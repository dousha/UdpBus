using System.Runtime.Serialization;

namespace UdpBus.Model;

public class BusEntry
{
    public BusEntry()
    {
    }

    public BusEntry(string? program, int inbound, int outbound) : this(program, inbound, outbound, string.Empty)
    {
    }

    public BusEntry(string? program, int inbound, int outbound, string? filter)
    {
        Program = program;
        InboundPort = inbound;
        OutboundPort = outbound;
        FilterValue = filter;
        if (Enum.TryParse(FilterValue, out FilterType parsedFilterType))
        {
            Filter = parsedFilterType;
        }
        else
        {
            Filter = FilterType.Allow;
            FilterValue = FilterType.Allow.ToString();
        }
    }

    public BusEntry(string? program, int inbound, int outbound, FilterType filter)
    {
        Program = program;
        InboundPort = inbound;
        OutboundPort = outbound;
        Filter = filter;
        FilterValue = filter.ToString();
    }

    [DataMember(Name = "program")] public string? Program { get; init; }

    [DataMember(Name = "inbound_port")] public int InboundPort { get; init; }

    [DataMember(Name = "outbound_port")] public int OutboundPort { get; init; }

    [DataMember(Name = "filter")] public string? FilterValue { get; init; }

    [IgnoreDataMember] public FilterType Filter { get; }

    public override bool Equals(object? obj)
    {
        return obj is BusEntry entry && Equals(entry);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Program, InboundPort, OutboundPort);
    }

    private bool Equals(BusEntry other)
    {
        return Program == other.Program && InboundPort == other.InboundPort && OutboundPort == other.OutboundPort;
    }
}
