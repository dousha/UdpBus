using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace UdpBusDriver.Converter;

[ValueConversion(typeof(string), typeof(string))]
public class FilterTypeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string key)
        {
            return "(???)";
        }

        var dict = Application.Current.Resources;
        var result = dict[key] as string ?? key;
        return result;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
