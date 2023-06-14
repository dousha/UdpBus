using System;
using System.Globalization;
using System.Windows.Data;

namespace UdpBusDriver.Converter;

[ValueConversion(typeof(bool), typeof(string))]
public class StartStopTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool b) return @"错误";

        return b ? @"停止" : @"启动";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
