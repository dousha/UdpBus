using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using UdpBus.Logic.ForwardCondition;
using UdpBus.Model;

namespace UdpBus.Logic;

internal class BusWorker
{
    private readonly UdpClient client;
    private readonly IForwardCondition forwardCondition;

    private readonly Hub hub;
    private readonly DatagramDirection inboundDirection;
    private readonly IPEndPoint outboundEndpoint;
    private readonly ConcurrentQueue<Datagram> packets;
    private readonly Semaphore pendingEventCount;
    private readonly UdpState state;
    private readonly Thread worker;
    private bool running;

    internal BusWorker(Hub hub, DatagramDirection direction, BusEntry entry)
    {
        this.hub = hub;
        inboundDirection = direction;
        forwardCondition = entry.Filter.GetFilterInstance<IForwardCondition>();

        var inboundEndpoint = IPAddress.TryParse(entry.InboundAddress, out var inboundAddress)
            ? new IPEndPoint(inboundAddress, entry.InboundPort)
            : new IPEndPoint(IPAddress.Any, entry.InboundPort);

        client = new UdpClient(inboundEndpoint);
        state = new UdpState
        {
            Client = client,
            Endpoint = inboundEndpoint
        };

        outboundEndpoint = IPAddress.TryParse(entry.OutboundAddress, out var outboundAddress)
            ? new IPEndPoint(outboundAddress, entry.OutboundPort)
            : new IPEndPoint(IPAddress.Loopback, entry.OutboundPort);

        packets = new ConcurrentQueue<Datagram>();
        pendingEventCount = new Semaphore(0, 255);

        running = false;
        worker = new Thread(WorkerLoop);
    }

    internal void Start()
    {
        running = true;
        worker.Start();
    }

    internal void Stop()
    {
        running = false;
        pendingEventCount.Release();
        worker.Join();
    }

    internal void Send(Datagram datagram)
    {
        packets.Enqueue(datagram);
        pendingEventCount.Release();
    }

    private void WorkerLoop()
    {
        client.BeginReceive(OnReceive, state);

        while (true)
        {
            pendingEventCount.WaitOne();
            if (!running)
            {
                client.Close();
                break;
            }

            if (!packets.TryDequeue(out var datagram)) continue;

            switch (datagram.Direction)
            {
                case DatagramDirection.FromDownstream:
                    hub.QueueToUpstream(datagram.Data);
                    client.BeginReceive(OnReceive, state);
                    break;
                case DatagramDirection.FromUpstream:
                    hub.QueueToDownstream(datagram.Data);
                    client.BeginReceive(OnReceive, state);
                    break;
                case DatagramDirection.ToDownstream:
                    goto case DatagramDirection.ToUpstream;
                case DatagramDirection.ToUpstream:
                    if (forwardCondition.CanForward(datagram)) client.Send(datagram.Data, outboundEndpoint);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void OnReceive(IAsyncResult result)
    {
        var s = (UdpState) result.AsyncState!;
        var e = s.Endpoint;

        try
        {
            var bytes = client.EndReceive(result, ref e);
            packets.Enqueue(new Datagram(inboundDirection, bytes));
        }
        catch (ObjectDisposedException)
        {
        }
        catch (SocketException)
        {
        }
        finally
        {
            pendingEventCount.Release();
        }
    }

    private struct UdpState
    {
        public UdpClient Client;
        public IPEndPoint Endpoint;
    }
}
