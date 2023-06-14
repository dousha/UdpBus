using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace UdpBusDriver.Converter;

[ValueConversion(typeof(bool), typeof(Brush))]
public class StateColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool b) return Brushes.Gray;

        return b ? Brushes.LimeGreen : Brushes.LightGray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
