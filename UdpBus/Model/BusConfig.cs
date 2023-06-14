using System.Runtime.Serialization;

namespace UdpBus.Model;

public class BusConfig
{
    public BusConfig()
    {
        Inbound = new List<BusEntry>();
        Outbound = new List<BusEntry>();
    }

    [DataMember(Name = "inbound")]
    public IList<BusEntry> Inbound { get; set; }

    [DataMember(Name = "outbound")]
    public IList<BusEntry> Outbound { get; set; }
}
