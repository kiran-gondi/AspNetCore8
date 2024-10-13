﻿using Microsoft.AspNetCore.Mvc;

namespace ViewsExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("home")]
        [Route("/")]
        public IActionResult Index()
        {
            return View();//Views/Home/Index.cshtml
            //return new ViewResult() { ViewName = "abc" };
        }
    }
}
