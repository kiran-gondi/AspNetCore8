using Microsoft.AspNetCore.Mvc;

namespace LayoutViewsExample.Controllers
{
    public class ProductsController : Controller
    {
        [Route("products")]
        public IActionResult Index()
        {
            return View();
        }

        //URL: /search-products/1
        [Route("search-products/{ProductID?}")]
        public IActionResult Search(int? ProductID)
        {
            ViewBag.ProductID = ProductID;  
            return View();
        }

        [Route("order-products")]
        public IActionResult Order()
        {
            return View();
        }
    }
}
