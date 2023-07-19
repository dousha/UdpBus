using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace UdpBusDriver.Converter;

[ValueConversion(typeof(int), typeof(string))]
public class EntryInboundPortConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var dict = Application.Current.Resources;
        var label = dict["LabelInboundPort"] as string;
        return value is not int port ? $"{label}???" : $"{label}{port}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
