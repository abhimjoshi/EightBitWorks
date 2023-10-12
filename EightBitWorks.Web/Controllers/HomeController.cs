using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using EightBitWorks.Web.Models;
using EightBitWorks.Web.Services.Mail;
using Microsoft.AspNetCore.OutputCaching;

namespace EightBitWorks.Web.Controllers;

public class HomeController : BaseController
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
    [OutputCache(Duration = 1800)]
    public IActionResult Index()
    {
        return View("Index");
    }
    
    [OutputCache(Duration = 1800)]
    public IActionResult Courses()
    {
        return View();
    }
    
    [OutputCache(Duration = 1800)]
    public IActionResult DockerCourse()
    {
        return View();
    }

    [OutputCache(Duration = 1800)]
    public IActionResult KubernetesCourse()
    {
        return View();
    }

    [OutputCache(Duration = 1800)]
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

    /// <summary>
    /// This endpoint is used to just keep the container awake.
    /// </summary>
    /// <returns></returns>
    [HttpGet("invalidate/cache")]
    public IActionResult CacheInvalidationOnly()
    {
        var invalidate = new
        {
            cacheInvalidated = true,
            dateTime = $"{DateTime.Now.ToUniversalTime().ToString(CultureInfo.InvariantCulture)} UTC"
        };

        return Json(invalidate);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}