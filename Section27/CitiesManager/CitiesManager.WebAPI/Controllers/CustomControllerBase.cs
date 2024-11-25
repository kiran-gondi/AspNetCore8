using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.WebAPI.Controllers
{
//[Route("api/v{version:apiVersion}/[controller]")] //UrlSegmentApiVersionReader
[Route("api/[controller]")] //QueryStringApiVersionReader
[ApiController]
 public class CustomControllerBase : ControllerBase
 {
 }
}

