using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace EightBitWorks.Web.Controllers;

public class ApiController : Controller
{
    /// <summary>
    /// This API method is called by TimedHostedService background job to keep the host up and running
    /// when deploying the web app on Render.com as a container application.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("get/time")]
    public IActionResult GetTime()
    {
        return Json(new { CurrentTime = DateTime.Now.ToString(CultureInfo.InvariantCulture) });
    }
}