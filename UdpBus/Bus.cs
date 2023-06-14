using Tomlyn;
using UdpBus.Logic;
using UdpBus.Model;

namespace UdpBus;

public class Bus
{
    public Bus(string configPath)
    {
        this.configPath = configPath;

        if (!File.Exists(configPath))
        {
            Config = new BusConfig();
            hub = new Hub();
            return;
        }

        var configText = File.ReadAllText(configPath);
        var success = Toml.TryToModel(configText, out BusConfig? config, out _);
        if (!success)
        {
            Config = new BusConfig();
            hub = new Hub();
            return;
        }

        if (config == null)
        {
            Config = new BusConfig();
            hub = new Hub();
            return;
        }

        Config = config;
        hub = new Hub();
    }

    public void Start()
    {
        hub.Start(Config);
        Started?.Invoke(this, EventArgs.Empty);
    }

    public void Stop()
    {
        // FIXME: use async call here, otherwise the UI would stuck
        hub.Stop();
        Stopped?.Invoke(this, StopReason.UserInitiated);
    }

    public void Save()
    {
        var data = Toml.FromModel(Config);
        File.WriteAllText(configPath, data);
    }

    public event EventHandler? Started;

    public event EventHandler<StopReason>? Stopped;

    public BusConfig Config
    {
        get;
        private set;
    }

    private readonly string configPath;
    private Hub hub;
}
