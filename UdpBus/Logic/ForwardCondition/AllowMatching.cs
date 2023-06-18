namespace UdpBus.Logic.ForwardCondition;

public abstract class AllowMatching : IForwardCondition
{
    public bool CanForward(Datagram datagram)
    {
        return DoesMatch(datagram.Data);
    }

    protected abstract bool DoesMatch(ReadOnlySpan<byte> data);
}
