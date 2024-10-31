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
        private readonly IServiceScopeFactory _serviceScopeFactory;

        //CONSTRUCTOR Injection
        public HomeController(ICitiesService citiesService1, ICitiesService citiesService2, 
            ICitiesService citiesService3, IServiceScopeFactory serviceScopeFactory)
        {
            //_citiesService = new CitiesService();
            _citiesService1 = citiesService1;
            _citiesService2 = citiesService2;
            _citiesService3 = citiesService3;
            _serviceScopeFactory = serviceScopeFactory;
        }


        [Route("/")]
        //public IActionResult Index([FromServices]ICitiesService _citiesService)//METHOD Injection
        public IActionResult Index()
        {
            List<string> cities = _citiesService1.GetCities();

            ViewBag.CitiesServiceInstanceId1 = _citiesService1.ServiceInstanceId;
            ViewBag.CitiesServiceInstanceId2 = _citiesService2.ServiceInstanceId;
            ViewBag.CitiesServiceInstanceId3 = _citiesService3.ServiceInstanceId;

            using(IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                //Inject CitiService
                ICitiesService citiesService = scope.ServiceProvider.GetService<ICitiesService>();
                //DB work

                ViewBag.CitiesServiceInstanceId_ServiceScope = citiesService.ServiceInstanceId;
            }//end of Scope/ it calls CitiesService.Dispose()

            return View(cities);
        }
    }
}
