using Microsoft.AspNetCore.Mvc;

namespace ConfigurationExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("/")]
        public IActionResult Index()
        {
            /*ViewBag.MyKey = _configuration["MYkey"];
            ViewBag.MyApiKey = _configuration.GetValue("MyApiKey", "the default key");

            ViewBag.ClientID = _configuration["weatherapi:ClientID"];
            ViewBag.ClientSecret = _configuration.GetValue("weatherapi:ClientSecret", "the default key");*/

            /*IConfiguration? weatherApiSection = _configuration.GetSection("weatherapi");
            ViewBag.ClientID = weatherApiSection["ClientID"];
            ViewBag.ClientSecret = weatherApiSection["ClientSecret"]; */

            //Bind: Loads configuration values into a new weatherApiOptions object.
            /*WeatherApiOptions? weatherApiOptions = _configuration.GetSection("weatherapi").Get<WeatherApiOptions>();
            ViewBag.ClientID = weatherApiOptions.ClientID;
            ViewBag.ClientSecret = weatherApiOptions.ClientSecret;*/

            //Bind: Loads configuration values into an existing weatherApiOptions object.
            WeatherApiOptions weatherApiOptions = new WeatherApiOptions();
            _configuration.GetSection("weatherapi").Bind(weatherApiOptions);

            ViewBag.ClientID = weatherApiOptions.ClientID;
            ViewBag.ClientSecret = weatherApiOptions.ClientSecret;

            return View();
        }
    }
}
