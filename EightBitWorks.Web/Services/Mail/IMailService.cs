namespace EightBitWorks.Web.Services.Mail;

public interface IMailService
{
    bool SendMail(MailData mailData);
}