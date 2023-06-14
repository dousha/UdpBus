namespace UdpBus.Exception;

public class InvalidConfigException : System.Exception
{
    public InvalidConfigException(string what)
    {
        this.what = what;
    }

    public override string ToString()
    {
        return $"Server cannot understand the configuration: {this.what}";
    }

    private readonly string what;
}
