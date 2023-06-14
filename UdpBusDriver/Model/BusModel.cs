using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UdpBus;
using UdpBus.Model;
using UdpBusDriver.Util;

namespace UdpBusDriver.Model;

public class BusModel : INotifyPropertyChanged
{
    private readonly Bus bus = new(FileUtil.GetDefaultConfigurationPath());

    private bool isRunning;
    private bool operationPending;

    public BusModel()
    {
        bus.Started += (_, _) =>
        {
            IsRunning = true;
            OperationPending = false;
        };

        bus.Stopped += (_, _) =>
        {
            IsRunning = false;
            OperationPending = false;
        };

        inbound = new ObservableCollection<BusEntry>(bus.Config.Inbound);
        outbound = new ObservableCollection<BusEntry>(bus.Config.Outbound);

        inbound.CollectionChanged += (_, _) =>
        {
            bus.Config.Inbound.Clear();
            foreach (var entry in inbound)
            {
                bus.Config.Inbound.Add(entry);
            }
            bus.Save();
        };

        outbound.CollectionChanged += (_, _) =>
        {
            bus.Config.Outbound.Clear();
            foreach (var entry in outbound)
            {
                bus.Config.Outbound.Add(entry);
            }
            bus.Save();
        };
    }

    public bool IsRunning
    {
        get => isRunning;
        set => SetField(ref isRunning, value);
    }

    public bool OperationPending
    {
        get => operationPending;
        set => SetField(ref operationPending, value);
    }

    public ObservableCollection<BusEntry> InboundEntries
    {
        get => inbound;
        set => SetField(ref inbound, value);
    }

    public ObservableCollection<BusEntry> OutboundEntries
    {
        get => outbound;
        set => SetField(ref outbound, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private ObservableCollection<BusEntry> inbound;

    private ObservableCollection<BusEntry> outbound;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    public void ToggleStartStop()
    {
        if (operationPending) return;

        OperationPending = true;

        if (isRunning)
            bus.Stop();
        else
            bus.Start();
    }

    public void ResetState()
    {
        OperationPending = false;
        IsRunning = false;
    }
}
