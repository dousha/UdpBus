namespace UdpBus.Logic.ForwardCondition;

public class AllowAny : IForwardCondition
{
    public bool CanForward(Datagram _)
    {
        return true;
    }
}
