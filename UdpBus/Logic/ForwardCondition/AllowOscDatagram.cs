using System.Text;
using System.Text.RegularExpressions;

namespace UdpBus.Logic.ForwardCondition;

public partial class AllowOscDatagram : AllowMatching
{
    private static readonly Regex Regex = MyRegex();

    protected override bool DoesMatch(ReadOnlySpan<byte> data)
    {
        if (data.Length % 4 != 0) return false;

        if (data[0] == '#')
        {
            // OSC Bundle
            var bundleHeader = Encoding.UTF8.GetString(data[..7]);
            return bundleHeader == "#bundle";
        }

        if (data[0] != '/') return false;

        // OSC message
        var stringTermination = data.IndexOf((byte) 0);
        var address = Encoding.UTF8.GetString(data[..stringTermination]);
        return Regex.IsMatch(address);
    }

    [GeneratedRegex("^[0-9A-Za-z%/]+$")]
    private static partial Regex MyRegex();
}
