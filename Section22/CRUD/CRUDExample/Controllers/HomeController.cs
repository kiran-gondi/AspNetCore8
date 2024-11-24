using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CRUDExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("Error")]
        public IActionResult Error()
        {
            IExceptionHandlerFeature? exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (exceptionHandlerFeature != null && exceptionHandlerFeature.Error != null) { 
            ViewBag.ErrorMessage = exceptionHandlerFeature.Error.Message;
            }
            return View();//Views/Shared/Error
        }
    }
}
