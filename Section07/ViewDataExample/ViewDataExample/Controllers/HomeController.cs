using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ViewDataExample.Models;

namespace ViewDataExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        List<Person> people = new List<Person>()
              {
                  new Person() { Name = "John", DateOfBirth = DateTime.Parse("2000-05-06"), PersonGender = Gender.Male},
                  new Person() { Name = "Linda", DateOfBirth = DateTime.Parse("2005-01-09"), PersonGender = Gender.Female},
                  new Person() { Name = "Susan", DateOfBirth = null, PersonGender = Gender.Other}
              };

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["appTitle"] = "Asp.Net Core Demo App!";

            //ViewData["people"] = people;
            //ViewBag.people = people;
            //return View(people);
            return View("Index", people);
        }

        [Route("person-details/{name}")]
        public IActionResult Details(string? name)
        {
            if(name == null)
            {
                return Content("Person name can't be null");
            }

           Person? matchPerson = people.Where(x=>x.Name == name).FirstOrDefault();
           return View(matchPerson);//View/Home/Details.cshtml
        }

        [Route("person-with-product")]
        public IActionResult PersonWithProduct() 
        { 
            Person p1 = new Person() { Name = "Linda1", DateOfBirth = DateTime.Parse("2005-01-09"), PersonGender = Gender.Female };
            Product product1 = new Product() { ProductId = 101, ProductName = "Bluetooth" };

            PersonAndProductWrapperModel personAndProductWrapperModel = new PersonAndProductWrapperModel()
            {
                PersonData = p1,
                ProductData = product1,
            };

            return View("PersonAndProduct", personAndProductWrapperModel); 
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
