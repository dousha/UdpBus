using System;
using System.Globalization;
using System.Windows.Data;

namespace UdpBusDriver.Converter;

[ValueConversion(typeof(bool), typeof(string))]
public class StateStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool b) return @"未知状态";

        return b ? @"正在运行" : @"未在运行";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
