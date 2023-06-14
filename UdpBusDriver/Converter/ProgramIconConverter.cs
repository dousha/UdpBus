using System;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace UdpBusDriver.Converter;

[ValueConversion(typeof(string), typeof(ImageSource))]
public class ProgramIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string path) return new BitmapImage();

        if (string.IsNullOrEmpty(path)) return new BitmapImage();

        using var ico = Icon.ExtractAssociatedIcon(path);
        return ico is null
            ? new BitmapImage()
            : Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
