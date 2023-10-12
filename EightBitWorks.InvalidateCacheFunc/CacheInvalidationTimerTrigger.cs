using System.Net.Http.Headers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace EightBitWorks.InvalidateCacheFunc;

public static class CacheInvalidationTimerTrigger
{
    private const string Url = "https://eightbitworks.com";
    private const string SubUrl = "/invalidate/cache";
    
    [Function("CacheInvalidationTimerTrigger")]
    public static void Run([TimerTrigger("0 */10 * * * *")] MyInfo myTimer, FunctionContext context)
    {
        var logger = context.GetLogger("CacheInvalidationTimerTrigger");
        var client = new HttpClient();

        try
        {
            client.BaseAddress = new Uri(Url);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            var response =
                client.GetAsync(SubUrl)
                    .Result; // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var result =
                    response.Content.ReadAsStringAsync()
                        .Result; //Make sure to add a reference to System.Net.Http.Formatting.dll

                logger.LogInformation(result);
            }
            else
            {
                logger.LogError("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now.ToUniversalTime()}");
            logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"There was an issue while making a REST api call: {ex.Message}");
        }
        finally
        {
            client.Dispose();
        }
        
        
        
    }
}

public class MyInfo
{
    public MyScheduleStatus ScheduleStatus { get; set; }

    public bool IsPastDue { get; set; }
}

public class MyScheduleStatus
{
    public DateTime Last { get; set; }

    public DateTime Next { get; set; }

    public DateTime LastUpdated { get; set; }
}