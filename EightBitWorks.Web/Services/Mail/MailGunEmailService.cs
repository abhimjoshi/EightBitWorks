using System.Text;
using Microsoft.Extensions.Options;
using RestSharp;

namespace EightBitWorks.Web.Services.Mail;

public class MailGunEmailService : IMailService
{
    private readonly RestClient restClient;
    private readonly MailGunSettings mailGunSettings;
    
    public MailGunEmailService(IOptions<MailGunSettings> mailSettingsOptions)
    {
        this.mailGunSettings = mailSettingsOptions.Value;
        
        var options = new RestClientOptions(this.mailGunSettings.BaseUrl)
        {
            ThrowOnAnyError = false
        };

        this.restClient = new RestClient(options);
    }
    
    public bool SendMail(MailData mailData)
    {
        var request = new RestRequest($"/v3/{this.mailGunSettings.Domain}/messages", Method.Post);

        // Basic Auth
        var authToken = Convert.ToBase64String(
            Encoding.ASCII.GetBytes($"api:{this.mailGunSettings.ApiKey}")
        );

        request.AddHeader("Authorization", $"Basic {authToken}");

        request.AddParameter("from", $"{this.mailGunSettings.SenderName} <{this.mailGunSettings.SenderEmail}>");
        request.AddParameter("to", mailData.EmailToId);
        request.AddParameter("subject", mailData.EmailSubject);
        //request.AddParameter("html", mailData.EmailBody);
        request.AddParameter("text", mailData.EmailBody);

        var response = this.restClient.ExecuteAsync(request).Result;
        
        if (!response.IsSuccessful)
        {
            throw new Exception(
                $"Mailgun send failed: {(int)response.StatusCode} {response.Content}");
        }

        return true;
    }
}