using System.Windows;
using System.Windows.Controls;

namespace UdpBusDriver.Component;

public partial class RunningState : UserControl
{
    public static readonly DependencyProperty IsRunningProperty =
        DependencyProperty.Register(nameof(IsRunning), typeof(bool), typeof(RunningState), new UIPropertyMetadata());

    public RunningState()
    {
        InitializeComponent();
    }

    public bool IsRunning
    {
        get => (bool) GetValue(IsRunningProperty);
        set => SetValue(IsRunningProperty, value);
    }
}
