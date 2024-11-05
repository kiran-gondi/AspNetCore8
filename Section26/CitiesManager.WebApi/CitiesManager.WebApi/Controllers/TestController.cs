using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.WebApi.Controllers
{
    public class TestController : CustomControllerBase
    {
        [HttpGet]
        public string Method()
        {
            return "Hello";
        }
    }
}
