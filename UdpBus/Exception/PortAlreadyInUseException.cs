using UdpBus.Model;

namespace UdpBus.Exception;

public class PortAlreadyInUseException : System.Exception
{
    public PortAlreadyInUseException(DatagramDirection which, int index, BusEntry entry)
    {
        Direction = which;
        Index = index;
        Entry = entry;
    }

    public DatagramDirection Direction { get; }

    public int Index { get; }

    public BusEntry Entry { get; }

    public int Port
    {
        get => Entry.InboundPort;
    }

    public override string Message => $"Port {Port} is already in use";
}
