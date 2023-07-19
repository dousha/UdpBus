using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace UdpBusDriver.Converter;

[ValueConversion(typeof(bool), typeof(string))]
public class StateStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var dict = Application.Current.Resources;

        if (value is not bool b) return dict["StateUnknown"];

        return b ? dict["StateActive"] : dict["StateIdle"];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
