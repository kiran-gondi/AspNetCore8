using Microsoft.AspNetCore.Mvc;

namespace EnvironmentsExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("/")]
        [Route("some-route")]
        public IActionResult Index()
        {
            //_webHostEnvironment.IsDevelopment()
            //_webHostEnvironment.EnvironmentName;
            ViewBag.CurrentEnvironment = _webHostEnvironment.EnvironmentName;
            
            return View();
        }

        [Route("some-route")]
        public IActionResult Index1()
        {
            return View();
        }
    }
}
