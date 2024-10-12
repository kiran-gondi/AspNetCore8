using ControllersExample.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControllersExample.Controllers
{
    //public class HomeController 
    [Controller]
    public class Home : Controller
    {
        /*[Route("home")]
        [Route("/")]
        public string Index()
        {
            return "Hello from Index";
        }*/

        /*[Route("home")]
        [Route("/")]
        public ContentResult Index()
        {
            return new ContentResult(){
                Content = "Hello from Index", 
                ContentType = "text/plain"
            };
        }*/

        [Route("home")]
        [Route("/")]
        public ContentResult Index()
        {
            //return Content("Hello from Index", "text/plain");
            return Content("<h1>Hello from Index</h1>", "text/html");
        }

        [Route("about")]
        public string About()
        {
            return "Hello from About";
        }

        //[Route("contact-us/{mobile:int:regex(^\\d{{10}}$)}")]
        [Route("contact-us/{mobile:regex(^\\d{{10}}$)}")]
        public string Contact()
        {
            return "Hello from Contact";
        }

        [Route("person")]
        public JsonResult Person()
        {
            //return "{ \"key\":\"value\"}";
            Person person = new Person() { Id = Guid.NewGuid(), FirstName = "Jim", LastName="Tim", Age = 34 };
            return new JsonResult(person);
        }
    }
}
