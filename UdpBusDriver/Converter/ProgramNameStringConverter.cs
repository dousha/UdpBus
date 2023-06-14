using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace UdpBusDriver.Converter;

[ValueConversion(typeof(string), typeof(string))]
public class ProgramNameStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string path) return @"（自定义项目）";

        return string.IsNullOrEmpty(path) ? @"（自定义项目）" : Path.GetFileName(path);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
