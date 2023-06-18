using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Ookii.Dialogs.Wpf;
using UdpBus.Model;

namespace UdpBusDriver.Dialog;

public partial class BusEntryEditDialog : INotifyPropertyChanged
{
    private readonly int index;

    private readonly ObservableCollection<BusEntry> xs;
    private FilterType filter;

    private int inboundPort;
    private int outboundPort;
    private string program;

    public BusEntryEditDialog(ObservableCollection<BusEntry> xs, int index)
    {
        this.xs = xs;
        this.index = index;
        var initialValue = xs[index];
        program = initialValue.Program ?? string.Empty;
        inboundPort = initialValue.InboundPort;
        outboundPort = initialValue.OutboundPort;
        filter = initialValue.Filter;

        Entry = new BusEntry(Program, InboundPort, OutboundPort);
        InitializeComponent();
    }


    public string Program
    {
        get => program;
        set => SetField(ref program, value);
    }

    public int InboundPort
    {
        get => inboundPort;
        set => SetField(ref inboundPort, value);
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
        DialogResult = true;
        Entry = new BusEntry(Program, InboundPort, OutboundPort, Filter);
        xs.RemoveAt(index);
        xs.Insert(index, Entry);
        Close();
    }

    private void OnCancelClick(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void OnWindowClosing(object? sender, CancelEventArgs e)
    {
    }

    private void OnDeleteClick(object sender, RoutedEventArgs e)
    {
        xs.RemoveAt(index);
        DialogResult = true;
        Close();
    }
}
