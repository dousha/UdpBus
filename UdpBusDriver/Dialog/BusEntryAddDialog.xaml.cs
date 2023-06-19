using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Ookii.Dialogs.Wpf;
using UdpBus.Model;

namespace UdpBusDriver.Dialog;

public partial class BusEntryAddDialog : INotifyPropertyChanged
{
    private FilterType filter;
    private string inboundAddress;
    private int inboundPort;
    private string outboundAddress;
    private int outboundPort;
    private string program;

    public BusEntryAddDialog()
    {
        program = string.Empty;
        inboundAddress = "0.0.0.0";
        outboundAddress = "127.0.0.1";
        Entry = new BusEntry();
        InitializeComponent();
    }

    public string Program
    {
        get => program;
        set => SetField(ref program, value);
    }

    public string InboundAddress
    {
        get => inboundAddress;
        set => SetField(ref inboundAddress, value);
    }

    public int InboundPort
    {
        get => inboundPort;
        set => SetField(ref inboundPort, value);
    }

    public string OutboundAddress
    {
        get => outboundAddress;
        set => SetField(ref outboundAddress, value);
    }

    public int OutboundPort
    {
        get => outboundPort;
        set => SetField(ref outboundPort, value);
    }

    public FilterType Filter
    {
        get => filter;
        set => SetField(ref filter, value);
    }

    public BusEntry Entry { get; private set; }

    public event PropertyChangedEventHandler? PropertyChanged;

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

    private void OnProgramOpenClick(object sender, RoutedEventArgs e)
    {
        var dialog = new VistaOpenFileDialog
        {
            ShowReadOnly = true,
            Filter = "Executable (*.exe)|*.exe",
            CheckFileExists = true,
            Multiselect = false
        };

        if (dialog.ShowDialog(this) is not true) return;

        Program = dialog.FileName;
    }

    private void OnSaveClick(object sender, RoutedEventArgs e)
    {
        if (InboundPort == 0 || OutboundPort == 0)
        {
            DialogResult = false;
            MessageBox.Show("入站端口和出站端口不能为 0");
        }
        else
        {
            DialogResult = true;
            Close();
        }
    }

    private void OnCancelClick(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void OnWindowClosing(object? sender, CancelEventArgs e)
    {
        Entry = new BusEntry(Program, InboundAddress, InboundPort, OutboundAddress, OutboundPort, Filter);
    }
}
