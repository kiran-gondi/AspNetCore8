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
            //return new JsonResult(person);
            return Json(person);
        }

        [Route("file-download1")]
        public VirtualFileResult FileDownload1()
        {
            //return new VirtualFileResult("/dummy-pdf_2.pdf", "application/pdf");
            return File("/dummy-pdf_2.pdf", "application/pdf");
        }

        [Route("file-download2")]
        public PhysicalFileResult FileDownload2()
        {
            //return new PhysicalFileResult("C:\\HandsOn\\U\\AspNetCore8\\Section06\\ControllersExample\\ControllersExample\\dummy-pdf_2 - Copy.pdf", "application/pdf");
            return PhysicalFile(@"C:\\HandsOn\\U\\AspNetCore8\\Section06\\ControllersExample\\ControllersExample\\dummy-pdf_2 - Copy.pdf", "application/pdf");
        }

        [Route("file-download3")]
        public FileContentResult FileDownload3()
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(@"C:\HandsOn\U\AspNetCore8\Section06\ControllersExample\ControllersExample\dummy-pdf_2 - Copy.pdf");
            //return new FileContentResult(fileBytes, "application/pdf");
            return File(fileBytes, "application/pdf");
        }

        [Route("file-download4")]
        public IActionResult FileDownload4()
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(@"C:\HandsOn\U\AspNetCore8\Section06\ControllersExample\ControllersExample\dummy-pdf_2 - Copy.pdf");
            //return new FileContentResult(fileBytes, "application/pdf");
            return File(fileBytes, "application/pdf");
        }
    }
}
