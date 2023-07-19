using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace UdpBusDriver.Converter;

[ValueConversion(typeof(bool), typeof(string))]
public class StartStopTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var dict = Application.Current.Resources;

        if (value is not bool b) return dict["BtnError"];

        return b ? dict["BtnStop"] : dict["BtnStart"];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
