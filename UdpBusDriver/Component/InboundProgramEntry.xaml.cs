﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using UdpBus.Model;
using UdpBusDriver.Dialog;

namespace UdpBusDriver.Component;

public partial class InboundProgramEntry : UserControl, INotifyPropertyChanged
{
    public static readonly DependencyProperty IndexProperty = DependencyProperty.Register(nameof(Index), typeof(int),
        typeof(InboundProgramEntry), new PropertyMetadata(0, OnIndexChange));

    public static readonly DependencyProperty ArrayProperty = DependencyProperty.Register(nameof(Entries),
        typeof(ObservableCollection<BusEntry>), typeof(InboundProgramEntry),
        new PropertyMetadata(new ObservableCollection<BusEntry>(), OnListChange));

    public InboundProgramEntry()
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

    private void OnEditClick(object sender, RoutedEventArgs e)
    {
        var dialog = new BusEntryEditDialog(Entries, Index);
        dialog.ShowDialog();
    }

    private string? program;
    private int inboundPort;
    private int outboundPort;

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

    private static void OnListChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is InboundProgramEntry c)
        {
            UpdateDisplay(c);
        }
    }

    private static void OnIndexChange(DependencyObject d, DependencyPropertyChangedEventArgs _)
    {
        if (d is InboundProgramEntry c)
        {
            UpdateDisplay(c);
        }
    }

    private static void UpdateDisplay(InboundProgramEntry c)
    {
        if (c.Entries.Count <= c.Index)
        {
            return;
        }

        var entry = c.Entries[c.Index];
        c.Program = entry.Program;
        c.InboundPort = entry.InboundPort;
        c.OutboundPort = entry.OutboundPort;
    }
}
