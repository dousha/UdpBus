namespace UdpBus.Exception;

public class InvalidConfigException : System.Exception
{
    private readonly string what;

    public InvalidConfigException(string what)
    {
        this.what = what;
    }

    public override string ToString()
    {
        return $"Server cannot understand the configuration: {what}";
    }
}
