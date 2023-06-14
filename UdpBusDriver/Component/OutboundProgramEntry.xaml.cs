using System.Windows;
using System.Windows.Controls;
using UdpBus.Model;

namespace UdpBusDriver.Component;

public partial class OutboundProgramEntry : UserControl
{
    public static readonly DependencyProperty EntryProperty = DependencyProperty.Register(nameof(Entry),
        typeof(BusEntry), typeof(OutboundProgramEntry), new PropertyMetadata(new BusEntry()));

    public static readonly DependencyProperty ProgramProperty = DependencyProperty.Register(nameof(Program),
        typeof(string), typeof(OutboundProgramEntry), new PropertyMetadata(""));

    public static readonly DependencyProperty InboundPortProperty =
        DependencyProperty.Register(nameof(InboundPort), typeof(int), typeof(OutboundProgramEntry),
            new PropertyMetadata(0));

    public static readonly DependencyProperty OutboundPortProperty =
        DependencyProperty.Register(nameof(OutboundPort), typeof(int), typeof(OutboundProgramEntry),
            new PropertyMetadata(0));

    public static readonly DependencyProperty IndexProperty = DependencyProperty.Register(nameof(Index), typeof(int),
        typeof(OutboundProgramEntry), new PropertyMetadata(0));

    public OutboundProgramEntry()
    {
        InitializeComponent();
    }

    public BusEntry Entry
    {
        get => (BusEntry) GetValue(EntryProperty);
        set => SetValue(EntryProperty, value);
    }

    public string? Program
    {
        get => (string?) GetValue(ProgramProperty);
        set => SetValue(ProgramProperty, value);
    }

    public int InboundPort
    {
        get => (int) GetValue(InboundPortProperty);
        set => SetValue(InboundPortProperty, value);
    }

    public int OutboundPort
    {
        get => (int) GetValue(OutboundPortProperty);
        set => SetValue(OutboundPortProperty, value);
    }

    public int Index
    {
        get => (int) GetValue(IndexProperty);
        set => SetValue(IndexProperty, value);
    }
}
