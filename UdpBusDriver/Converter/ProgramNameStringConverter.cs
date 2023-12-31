﻿using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;

namespace UdpBusDriver.Converter;

[ValueConversion(typeof(string), typeof(string))]
public class ProgramNameStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var dict = Application.Current.Resources;
        var label = dict["LabelUnknownApplication"] as string ?? string.Empty;

        if (value is not string path) return label;

        return string.IsNullOrEmpty(path) ? label : Path.GetFileName(path);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
