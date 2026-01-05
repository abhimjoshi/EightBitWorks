namespace EightBitWorks.Web.Services.Mail;

public class MailGunSettings
{
    public string ApiKey { get; set; }
    public string Domain { get; set; }
    public string BaseUrl { get; set; }
    public string SenderName { get; set; }
    public string SenderEmail { get; set; }
}