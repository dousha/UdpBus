using System;
using System.Globalization;
using System.Windows.Data;

namespace UdpBusDriver.Converter;

[ValueConversion(typeof(int), typeof(string))]
public class EntryOutboundPortConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is not int port ? @"出站端口：???" : $"出站端口：{port}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
