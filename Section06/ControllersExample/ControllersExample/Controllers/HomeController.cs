using Microsoft.AspNetCore.Mvc;

namespace ControllersExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("sayHello")]
        public string method1()
        {
            return "Hello from method1";
        }
    }
}
