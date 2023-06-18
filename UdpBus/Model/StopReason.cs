namespace UdpBus.Model;

public enum StopReason
{
    UserInitiated = 0,
    PortAlreadyInUse = 1,
    SystemError = 2,
    InvalidConfig = 3,
    Unknown = 0xff
}
