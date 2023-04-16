using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Stock_App.Controllers
{
    public class HomeController : Controller
    {
        [Route("Error")]
        public IActionResult Error()
        {
            IExceptionHandlerPathFeature? exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if(exceptionHandlerPathFeature is not null && exceptionHandlerPathFeature.Path is not null)
            {
                ViewBag.ErrorMessage = exceptionHandlerPathFeature.Error.Message;
			}
            return View();
        }
    }
}
