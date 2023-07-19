using System;
using System.Threading;
using System.Windows;

namespace UdpBusDriver;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        var languageName = Thread.CurrentThread.CurrentCulture.ToString();
        var dict = new ResourceDictionary();

        switch (languageName)
        {
            case "zh-CN":
                dict.Source = new Uri(@"..\I18N\Strings.zh-Hans.xaml", UriKind.Relative);
                break;
            default:
                dict.Source = new Uri(@"..\I18N\Strings.xaml", UriKind.Relative);
                break;
        }

        Resources.MergedDictionaries.Add(dict);
    }
}
