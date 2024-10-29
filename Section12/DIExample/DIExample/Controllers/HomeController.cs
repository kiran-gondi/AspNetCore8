using Microsoft.AspNetCore.Mvc;
using Services;
using ServiceContracts;

namespace DIExample.Controllers
{
    public class HomeController : Controller
    {
        //private readonly CitiesService _citiesService;
        private readonly ICitiesService _citiesService;

        public HomeController(ICitiesService citiesService)
        {
            //_citiesService = new CitiesService();
            _citiesService = citiesService;
        }

        [Route("/")]
        public IActionResult Index()
        {
            List<string> cities = _citiesService.GetCities();
            return View(cities);
        }
    }
}
