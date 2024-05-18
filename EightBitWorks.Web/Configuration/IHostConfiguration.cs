namespace EightBitWorks.Web.Configuration;

public interface IHostConfiguration
{
    public string ApiHostUrl { get; init; }
    
    public string GetResourceUrl(string resourcePath);

}