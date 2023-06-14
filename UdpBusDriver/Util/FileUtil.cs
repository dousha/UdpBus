using System;
using System.IO;

namespace UdpBusDriver.Util;

public static class FileUtil
{
    public static string GetAppDataBase()
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "UdpBus");
    }

    public static void EnsureAppDataBase()
    {
        if (!Directory.Exists(GetAppDataBase())) Directory.CreateDirectory(GetAppDataBase());
    }

    public static string GetDefaultConfigurationPath()
    {
        EnsureAppDataBase();
        return Path.Combine(GetAppDataBase(), "config.toml");
    }
}
