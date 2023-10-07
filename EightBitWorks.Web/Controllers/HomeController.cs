using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EightBitWorks.Web.Models;

namespace EightBitWorks.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    
    [ActionName("home")]
    public IActionResult Index()
    {
        return View("index");
    }
    
    public IActionResult Courses()
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