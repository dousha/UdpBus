namespace UdpBus.Model;

public enum DatagramDirection
{
    /// <summary>
    /// From downstream application to this app
    /// Forward to all upstream applications
    /// </summary>
    FromDownstream,
    /// <summary>
    /// Outbound packets to all downstream applications
    /// </summary>
    ToDownstream,
    /// <summary>
    /// From upstream application to this app
    /// Forward to all downstream applications
    /// </summary>
    FromUpstream,
    /// <summary>
    /// Outbound packets to all upstream applications
    /// </summary>
    ToUpstream
}
