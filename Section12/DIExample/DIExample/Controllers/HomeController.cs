using Microsoft.AspNetCore.Mvc;
using Services;
using ServiceContracts;

namespace DIExample.Controllers
{
    public class HomeController : Controller
    {
        //private readonly CitiesService _citiesService;
        private readonly ICitiesService _citiesService1;
        private readonly ICitiesService _citiesService2;
        private readonly ICitiesService _citiesService3;

        //CONSTRUCTOR Injection
        public HomeController(ICitiesService citiesService1, ICitiesService citiesService2, ICitiesService citiesService3)
        {
            //_citiesService = new CitiesService();
            _citiesService1 = citiesService1;
            _citiesService2 = citiesService2;
            _citiesService3 = citiesService3;
        }


        [Route("/")]
        //public IActionResult Index([FromServices]ICitiesService _citiesService)//METHOD Injection
        public IActionResult Index()
        {
            List<string> cities = _citiesService1.GetCities();

            ViewBag.CitiesServiceInstandId1 = _citiesService1.ServiceInstanceId;
            ViewBag.CitiesServiceInstandId2 = _citiesService2.ServiceInstanceId;
            ViewBag.CitiesServiceInstandId3 = _citiesService3.ServiceInstanceId;

            return View(cities);
        }
    }
}
