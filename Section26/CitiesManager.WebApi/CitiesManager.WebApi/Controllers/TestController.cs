using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public string Method()
        {
            return "Hello";
        }
    }
}
