using Assignment18.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment18.Controllers
{
    public class CityWeatherController : Controller
    {
        List<CityWeather> cityWeatherList = new List<CityWeather>()
        {
            new CityWeather() { CityUniqueCode = "LDN", CityName = "London",
                DateAndTime = Convert.ToDateTime("2030-01-01 8:00"),  TemperatureFahrenheit = 33 },
            new CityWeather() { CityUniqueCode = "NYC", CityName = "London",
                DateAndTime = Convert.ToDateTime("2030-01-01 3:00"),  TemperatureFahrenheit = 60 },
            new CityWeather() { CityUniqueCode = "PAR", CityName = "Paris",
                DateAndTime = Convert.ToDateTime("2030-01-01 9:00"),  TemperatureFahrenheit = 82 },

        };

        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            return View(cityWeatherList);
        }
        
        [HttpGet]
        [Route("/weather/{cityCode?}")]
        public IActionResult CityWeather(string? cityCode)
        {
            if (string.IsNullOrEmpty(cityCode))
            {
                return View(new CityWeather());
            }

            CityWeather? cityWeather = cityWeatherList.Where(x=>x.CityUniqueCode == cityCode).FirstOrDefault();
            return View(cityWeather);
        }
    }
}
