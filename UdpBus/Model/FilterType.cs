using UdpBus.Logic.ForwardCondition;

namespace UdpBus.Model;

public enum FilterType
{
    [FilterClass(typeof(AllowAny))] Allow,

    [FilterClass(typeof(AllowOscDatagram))]
    Osc,

    [FilterClass(typeof(DenyAny))] Deny
}
