using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EightBitWorks.Web.Services.Mail;

public class MailService : IMailService
{
    private readonly MailSettings _mailSettings;
    
    public MailService(IOptions<MailSettings> mailSettingsOptions)
    {
        _mailSettings = mailSettingsOptions.Value;
    }

    public bool SendMail(MailData mailData)
    {
        try
        {
            using MimeMessage emailMessage = new MimeMessage();
            var emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
            emailMessage.From.Add(emailFrom);
            var emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
            emailMessage.To.Add(emailTo);

            //emailMessage.Cc.Add(new MailboxAddress("Cc Receiver", "cc@example.com"));
            //emailMessage.Bcc.Add(new MailboxAddress("Bcc Receiver", "bcc@example.com"));

            emailMessage.Subject = mailData.EmailSubject;

            var emailBodyBuilder = new BodyBuilder
            {
                TextBody = mailData.EmailBody
            };

            emailMessage.Body = emailBodyBuilder.ToMessageBody();
            
            //this is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
            using var mailClient = new SmtpClient();
            mailClient.Connect(_mailSettings.Server, _mailSettings.Port, SecureSocketOptions.StartTls);
            mailClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
            mailClient.Send(emailMessage);
            mailClient.Disconnect(true);

            return true;
        }
        catch
        {
            return false;
        }
    }
}