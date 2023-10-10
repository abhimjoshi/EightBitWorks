using EightBitWorks.Web.Models;

namespace EightBitWorks.Web.Services.Mail;

public static class EmailTemplate
{
    public static string GetContactFormEmailBody(ContactFormModel model)
    {
        return $"""
               New Email Received:
               ----------------------
               Name: {model.Name}
               Company: {model.Company}
               From Email: {model.FromEmail}
               Message: 
               {model.Message}
               """;
    }

}