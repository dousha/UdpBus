namespace UdpBus.Logic.ForwardCondition;

public interface IForwardCondition
{
    public bool CanForward(Datagram datagram);
}
