namespace EightBitWorks.Web.Configuration;

public class HostConfiguration : IHostConfiguration
{
    public string CdnHostUrl { get; init; }
    
    public string ApiHostUrl { get; init; }

    public string GetResourceUrl(string resourcePath)
    {
        return $"{CdnHostUrl}{resourcePath}";
    }
}