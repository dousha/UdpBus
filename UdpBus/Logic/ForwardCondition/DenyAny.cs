namespace UdpBus.Logic.ForwardCondition;

public class DenyAny : IForwardCondition
{
    public bool CanForward(Datagram datagram)
    {
        return false;
    }
}
