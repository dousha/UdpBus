using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using UdpBus.Model;
using UdpBusDriver.Dialog;

namespace UdpBusDriver.Component;

public partial class ProgramEntry : UserControl, INotifyPropertyChanged
{
    public static readonly DependencyProperty IndexProperty = DependencyProperty.Register(nameof(Index), typeof(int),
        typeof(ProgramEntry), new PropertyMetadata(0, OnIndexChange));

    public static readonly DependencyProperty ArrayProperty = DependencyProperty.Register(nameof(Entries),
        typeof(ObservableCollection<BusEntry>), typeof(ProgramEntry),
        new PropertyMetadata(new ObservableCollection<BusEntry>(), OnListChange));

    private int inboundPort;
    private int outboundPort;

    private string? program;

    public ProgramEntry()
    {
        InitializeComponent();
    }

    public string? Program
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

    public ObservableCollection<BusEntry> Entries
    {
        get => (ObservableCollection<BusEntry>) GetValue(ArrayProperty);
        set => SetValue(ArrayProperty, value);
    }

    public int Index
    {
        get => (int) GetValue(IndexProperty);
        set => SetValue(IndexProperty, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnEditClick(object sender, RoutedEventArgs e)
    {
        var dialog = new BusEntryEditDialog(Entries, Index);
        dialog.ShowDialog();
    }

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

    private static void OnListChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ProgramEntry c) UpdateDisplay(c);
    }

    private static void OnIndexChange(DependencyObject d, DependencyPropertyChangedEventArgs _)
    {
        if (d is ProgramEntry c) UpdateDisplay(c);
    }

    private static void UpdateDisplay(ProgramEntry c)
    {
        if (c.Entries.Count <= c.Index) return;

        var entry = c.Entries[c.Index];
        c.Program = entry.Program;
        c.InboundPort = entry.InboundPort;
        c.OutboundPort = entry.OutboundPort;
    }
}
