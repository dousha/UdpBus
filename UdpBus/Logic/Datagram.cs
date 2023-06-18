using UdpBus.Model;

namespace UdpBus.Logic;

public struct Datagram
{
    public Datagram(DatagramDirection direction, ReadOnlySpan<byte> data)
    {
        Direction = direction;
        this.data = new byte[data.Length];
        data.CopyTo(this.data);
    }

    public ReadOnlySpan<byte> Data => data;

    public DatagramDirection Direction { get; }

    private readonly byte[] data;
}
