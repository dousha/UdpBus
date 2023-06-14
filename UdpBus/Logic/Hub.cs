using System.Collections.Concurrent;
using UdpBus.Exception;
using UdpBus.Model;

namespace UdpBus.Logic;

public class Hub
{
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
        // FIXME: what if the stuff is already running?
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

    private bool running;
    private readonly ConcurrentQueue<Datagram> packets;
    private readonly Semaphore pendingEventCount;
    private Thread? worker;
    private List<BusWorker> downstream;
    private List<BusWorker> upstream;
}
