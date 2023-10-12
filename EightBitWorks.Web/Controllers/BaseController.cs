using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EightBitWorks.Web.Controllers;

public class BaseController : Controller
{
    private const string SchemeHttp = "http";
    private const string SchemeHttps = "https";
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var httpRequest = context.HttpContext.Request;
        var isSecure = context.HttpContext.Request.IsHttps;
        var requestScheme = isSecure ? SchemeHttps : SchemeHttp;
        this.ViewData["PageUrl"] = $"{requestScheme}://{httpRequest.Host.Value}{httpRequest.Path.ToString()}";
        
        base.OnActionExecuting(context);
    }
}