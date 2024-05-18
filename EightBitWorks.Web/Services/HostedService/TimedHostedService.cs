

using EightBitWorks.Web.Configuration;
using RestSharp;

namespace EightBitWorks.Web.Services.HostedService;

public class TimedHostedService :  IHostedService, IDisposable
{
    private int _executionCount = 0;
    private ILogger<TimedHostedService> Logger { get; set; }
    private Timer Timer { get; set; }
    
    private IHostConfiguration HostConfiguration { get; }

    public TimedHostedService(ILogger<TimedHostedService> logger, IHostConfiguration hostConfiguration)
    {
        this.Logger = logger;
        this.HostConfiguration = hostConfiguration;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        this.Logger.LogInformation("Timed Hosted Service starting..");

        this.Timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));

        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        var count = Interlocked.Increment(ref _executionCount);

        this.Logger.LogInformation("Scheduler is running at {S}", DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss"));
        
        var restOption = new RestClientOptions(this.HostConfiguration.ApiHostUrl);
        var restClient = new RestClient(restOption);
        var restRequest = new RestRequest("get/time");
        var responseData = restClient.Get(restRequest).Content;

        this.Logger.LogInformation("Api response data: {ResponseData}, total scheduling count: {Count}", responseData,
            count);        
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        this.Logger.LogInformation("Timed Hosted Service is stopping");

        this.Timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        this.Timer?.Dispose();
    }
}