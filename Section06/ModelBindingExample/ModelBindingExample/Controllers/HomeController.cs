using Microsoft.AspNetCore.Mvc;
using ModelBindingExample.Models;

namespace ModelBindingExample.Controllers
{
    public class HomeController : Controller
    {
        /*[Route("bookstore/{bookid?}/{isLoggedin?}")]
        //URL: /bookstore?bookid=10&isloggedin=true
        public IActionResult Index(int? bookid, bool? isLoggedin)
        {
            if (bookid.HasValue == false)
            {
                return BadRequest("Book id is not supplied or empty");
            }

            if (bookid <= 0) {
                return BadRequest("Book id can't be less than or equal to 0");
            }

            if(bookid > 1000)
            {
                return NotFound("Book id can't be greater than 1000");
            }

            if (isLoggedin == false) {
                return StatusCode(401);
            }

            return Content($"Book id: {bookid}");
        }*/

        /*[Route("bookstore/{bookid?}/{isLoggedin?}")]
        //URL: /bookstore?bookid=10&isloggedin=true
        //public IActionResult Index([FromRoute]int? bookid, [FromRoute] bool? isLoggedin)
        public IActionResult Index([FromQuery]int? bookid, [FromQuery] bool? isLoggedin)
        {
            if (bookid.HasValue == false)
            {
                return BadRequest("Book id is not supplied or empty");
            }

            if (bookid <= 0)
            {
                return BadRequest("Book id can't be less than or equal to 0");
            }

            if (bookid > 1000)
            {
                return NotFound("Book id can't be greater than 1000");
            }

            if (isLoggedin == false)
            {
                return StatusCode(401);
            }

            return Content($"Book id: {bookid}");
        }*/

        [Route("bookstore/{bookid?}/{isLoggedin?}")]
        //URL: /bookstore?bookid=10&isloggedin=true
        public IActionResult Index([FromQuery]int? bookid, [FromRoute]bool? isLoggedin, Book book)
        {
            if (bookid.HasValue == false)
            {
                return BadRequest("Book id is not supplied or empty");
            }

            if (bookid <= 0)
            {
                return BadRequest("Book id can't be less than or equal to 0");
            }

            if (bookid > 1000)
            {
                return NotFound("Book id can't be greater than 1000");
            }

            if (isLoggedin == false)
            {
                return StatusCode(401);
            }

            return Content($"Book id: {bookid}, Book: {book.ToString()}");
        }
    }
}
