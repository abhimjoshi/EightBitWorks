using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EightBitWorks.Web.Controllers;

public class BaseController : Controller
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var httpRequest = context.HttpContext.Request;
        this.ViewData["PageUrl"] = $"{httpRequest.Scheme}://{httpRequest.Host.Value}{httpRequest.Path.ToString()}";
        
        base.OnActionExecuting(context);
    }
}