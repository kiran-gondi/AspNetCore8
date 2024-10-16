using Microsoft.AspNetCore.Mvc;
using PartialViewsExample.Models;

namespace PartialViewsExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            //ViewData["ListTitle"] = "Cities";
            //ViewData["ListItems"] = new List<string>() { 
            //    "Paris", "Newyork", "Navi Mumbai", "Rome"
            //};
            return View();
        }

        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("programming-languages")]
        public IActionResult ProgrammingLanguage()
        {
            ListModel listModel = new ListModel();
            listModel.ListTitle = "Programming Languages List";
            listModel.ListItems = new List<string>(){
                "Phython", "Go", "C#" 
            };

            //return new PartialViewResult() { ViewName = "_ListParitalView", Model = listModel};
            return PartialView("_ListPartialView", listModel);
        }
    }
}
