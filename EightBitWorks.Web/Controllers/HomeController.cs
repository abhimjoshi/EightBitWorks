using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using EightBitWorks.Web.Models;
using EightBitWorks.Web.Services.Mail;

namespace EightBitWorks.Web.Controllers;

public class HomeController : Controller
{
    private MailSettings MailSettings { get; }
    private ILogger<HomeController> Logger { get; }
    private IMailService MailService { get; }

    public HomeController(ILogger<HomeController> logger, IMailService mailService, 
        IOptions<MailSettings> mailSettingsOptions)
    {
        this.Logger = logger;
        this.MailService = mailService;
        this.MailSettings = mailSettingsOptions.Value;
    }

    
    [ActionName("Home")]
    public IActionResult Index()
    {
        return View("Index");
    }
    
    public IActionResult Courses()
    {
        return View();
    }
    
    public IActionResult DockerCourse()
    {
        return View();
    }

    public IActionResult KubernetesCourse()
    {
        return View();
    }

    public IActionResult AboutAuthor()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult ContactUs(ContactFormModel model)
    {
        if (this.ModelState.IsValid)
        {
            MailService.SendMail(new MailData
            {
                EmailBody = EmailTemplate.GetContactFormEmailBody(model),
                EmailSubject = $"New email from {model.Name}",
                EmailToId = this.MailSettings.SenderEmail,
                EmailToName = "EightBitWorks"
            });

            this.TempData["IsEmailSent"] = true;
            return RedirectToRoute("thank-you");
        }
        else
        {
            return View("Index");    
        }
    }
    public IActionResult ThankYou()
    {
        if (this.TempData["IsEmailSent"] != null && Convert.ToBoolean(this.TempData["IsEmailSent"]))
        {
            return View();
        }
        else
        {
            return RedirectToAction("Home");
        }
    }

    public IActionResult HttpCode404()
    {
        return View();
    }
    
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}