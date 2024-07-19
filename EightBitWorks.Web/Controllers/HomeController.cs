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
    private const string SecretKey = "6Lfh9RMqAAAAAOprpQ_tJ53xHGN0fL6Q5Qp7n2e2";
    
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ContactUs(ContactFormModel model)
    {
        var recaptchaResponse = this.HttpContext.Request.Form["g-recaptcha-response"];
        var isCaptchaVerified = await this.VerifyAsync(recaptchaResponse);
        
        if (this.ModelState.IsValid && isCaptchaVerified)
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

        this.ViewBag.CaptchaError = "Error: There was an issue with the submitted form. Please try again.";
        return View();
    }

    public async Task<bool> VerifyAsync(string token)
    {
        using var client = new HttpClient();
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("secret", SecretKey),
            new KeyValuePair<string, string>("response", token)
        });

        var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            // Deserialize JSON response from Google reCAPTCHA API
            var result = System.Text.Json.JsonSerializer.Deserialize<RecaptchaResponse>(responseString);
            // Check if the reCAPTCHA was successful
            return result.Success;
        }
        else
        {
            this.Logger.LogWarning($"reCAPTCHA verification request failed: {response.StatusCode}");
        }

        return false;
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

    public IActionResult ContactUs()
    {
        return View();
    }

    public IActionResult HttpCode404()
    {
        return View();
    }

    //[OutputCache(Duration = 1800)]
    [Route("sitemap.xml")]
    public IActionResult GetSitemap()
    {
        var fileBytes = System.IO.File.ReadAllBytes("sitemap.xml");
        return File(fileBytes, "text/xml");
    }

    [Route("robots.txt")]
    public IActionResult GetRobotsTxt()
    {
        var fileBytes = System.IO.File.ReadAllBytes("robots.txt");
        return File(fileBytes, "text/plain");
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


