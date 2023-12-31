﻿using System;
using System.ComponentModel;
using System.Windows;
using UdpBus.Exception;
using UdpBusDriver.Dialog;
using UdpBusDriver.Model;

namespace UdpBusDriver;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnStartStopClick(object sender, RoutedEventArgs e)
    {
        var bus = Resources["Bus"] as BusModel;
        try
        {
            bus!.ToggleStartStop();
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case InvalidConfigException:
                    MessageBox.Show(this, @"配置无效", string.Empty, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
                case PortAlreadyInUseException portAlreadyInUseException:
                    MessageBox.Show(this, $"入站端口 {portAlreadyInUseException.Port} 已经被占用，请调整到其他空闲端口", string.Empty,
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
                default:
                    MessageBox.Show(this, ex.Message, string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }

            bus!.ResetState();
        }
    }

    private void OnDownstreamCreateClick(object sender, RoutedEventArgs e)
    {
        var dlg = new BusEntryAddDialog();
        if (dlg.ShowDialog() is not true) return;

        var entry = dlg.Entry;
        var bus = Resources["Bus"] as BusModel;
        bus!.InboundEntries.Add(entry);
    }

    private void OnUpstreamCreateClick(object sender, RoutedEventArgs e)
    {
        var dlg = new BusEntryAddDialog();
        if (dlg.ShowDialog() is not true) return;

        var entry = dlg.Entry;
        var bus = Resources["Bus"] as BusModel;
        bus!.OutboundEntries.Add(entry);
    }

    private void OnWindowClosing(object? sender, CancelEventArgs e)
    {
        var bus = Resources["Bus"] as BusModel;
        if (bus?.IsRunning ?? false) bus.ToggleStartStop();
    }
}
