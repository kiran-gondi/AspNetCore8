using Microsoft.AspNetCore.Mvc;

namespace ModelBindingExample.Controllers
{
    public class StoreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
