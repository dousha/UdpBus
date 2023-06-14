using System.Runtime.Serialization;

namespace UdpBus.Model;

public class BusEntry
{
    public BusEntry()
    {
    }

    public BusEntry(string? program, int inbound, int outbound)
    {
        Program = program;
        InboundPort = inbound;
        OutboundPort = outbound;
    }

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

    [DataMember(Name = "program")] public string? Program { get; init; }

    [DataMember(Name = "inbound_port")] public int InboundPort { get; init; }

    [DataMember(Name = "outbound_port")] public int OutboundPort { get; init; }
}
