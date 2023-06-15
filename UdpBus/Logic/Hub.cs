using System.Collections.Concurrent;
using System.Net.NetworkInformation;
using UdpBus.Exception;
using UdpBus.Model;

namespace UdpBus.Logic;

public class Hub
{
    private readonly ConcurrentQueue<Datagram> packets;
    private readonly Semaphore pendingEventCount;
    private List<BusWorker> downstream;

    private bool running;
    private List<BusWorker> upstream;
    private Thread? worker;

    public Hub()
    {
        packets = new ConcurrentQueue<Datagram>();
        running = true;
        pendingEventCount = new Semaphore(0, 255);

        downstream = new List<BusWorker>();
        upstream = new List<BusWorker>();
    }

    internal void QueueToDownstream(ReadOnlySpan<byte> data)
    {
        packets.Enqueue(new Datagram(DatagramDirection.ToDownstream, data));
        pendingEventCount.Release();
    }

    internal void QueueToUpstream(ReadOnlySpan<byte> data)
    {
        packets.Enqueue(new Datagram(DatagramDirection.ToUpstream, data));
        pendingEventCount.Release();
    }

    public void Start(BusConfig config)
    {
        for (var i = 0; i < config.Inbound.Count; i++)
        {
            if (CheckIfPortIsUsed(config.Inbound[i].InboundPort))
            {
                throw new PortAlreadyInUseException(DatagramDirection.FromDownstream, i, config.Inbound[i]);
            }
        }

        for (var i = 0; i < config.Outbound.Count; i++)
        {
            if (CheckIfPortIsUsed(config.Outbound[i].InboundPort))
            {
                throw new PortAlreadyInUseException(DatagramDirection.FromUpstream, i, config.Outbound[i]);
            }
        }

        // FIXME: do i have to dispose these workers manually?
        downstream = config.Inbound.Select(it =>
            new BusWorker(this, DatagramDirection.FromDownstream, it.InboundPort, it.OutboundPort)).ToList();
        upstream = config.Outbound.Select(it =>
            new BusWorker(this, DatagramDirection.FromUpstream, it.InboundPort, it.OutboundPort)).ToList();
        Start();
    }

    private void Start()
    {
        if (downstream.Count == 0 || upstream.Count == 0)
        {
            // invalid configuration
            throw new InvalidConfigException("There is no valid upstream or downstream to talk to");
        }

        foreach (var busWorker in upstream)
        {
            busWorker.Start();
        }

        foreach (var busWorker in downstream)
        {
            busWorker.Start();
        }

        running = true;
        worker = new Thread(ProcessDataTransfer);
        worker.Start();
    }

    public void Stop()
    {
        // FIXME: can we call all the Close() at once?
        foreach (var busWorker in downstream)
        {
            busWorker.Stop();
        }

        foreach (var busWorker in upstream)
        {
            busWorker.Stop();
        }

        running = false;
        pendingEventCount.Release();
        worker!.Join();
    }

    private void ProcessDataTransfer()
    {
        while (true)
        {
            pendingEventCount.WaitOne();
            if (!running)
            {
                break;
            }

            if (!packets.TryDequeue(out var datagram))
            {
                continue;
            }

            switch (datagram.Direction)
            {
                case DatagramDirection.ToDownstream:
                {
                    foreach (var busWorker in downstream)
                    {
                        busWorker.Send(datagram);
                    }

                    break;
                }
                case DatagramDirection.ToUpstream:
                {
                    foreach (var busWorker in upstream)
                    {
                        busWorker.Send(datagram);
                    }

                    break;
                }
                case DatagramDirection.FromDownstream:
                    goto case default;
                case DatagramDirection.FromUpstream:
                    goto case default;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private static bool CheckIfPortIsUsed(int port)
    {
        return IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners().Any(p => p.Port == port);
    }
}
