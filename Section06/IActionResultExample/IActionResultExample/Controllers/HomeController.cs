using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Controllers
{
    public class HomeController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        /*        public ContentResult Index()
                {
                    if (Request.Query.ContainsKey("bookid"))
                    {
                        return Content("Book id is not supplied.");
                    }

                    //Book id can't be empty
                    if (string.IsNullOrEmpty(Convert.ToString(Request.Query["bookid"])))
                    {
                        return Content("Book id can't be null or empty");
                    }

                    //Book id should be between 1 to 100
                    int bookId = Convert.ToInt32(ControllerContext.HttpContext.Request.Query["bookid"]);
                    if (bookId <= 0)
                    {
                        return Content("Book id can't be less than or equal to zero");
                    }

                    if (bookId > 1000)
                    {
                        return Content("Book id can't be greater than 1000");
                    }

                    return File("/dummy.pdf", "application/pdf");
                }*/

        /*[Route("book")]
        public IActionResult Index()
        {
            if (!Request.Query.ContainsKey("bookid"))
            {
                Response.StatusCode = 400;
                return Content("Book id is not supplied.");
            }

            //Book id can't be empty
            if (string.IsNullOrEmpty(Convert.ToString(Request.Query["bookid"])))
            {
                Response.StatusCode = 400;
                return Content("Book id can't be null or empty");
            }

            //Book id should be between 1 to 100
            int bookId = Convert.ToInt32(ControllerContext.HttpContext.Request.Query["bookid"]);
            if (bookId <= 0)
            {
                Response.StatusCode = 400;
                return Content("Book id can't be less than or equal to zero");
            }

            if (bookId > 1000)
            {
                Response.StatusCode = 400;
                return Content("Book id can't be greater than 1000");
            }

            //isLoggedin should be true
            if (!Convert.ToBoolean(Request.Query["isLoggedin"]))
            {
                Response.StatusCode = 401;
                return Content("User must be authenticated");
            }

            return File("/dummy.pdf", "application/pdf");
        }*/

        [Route("book")]
        public IActionResult Index()
        {
            if (!Request.Query.ContainsKey("bookid"))
            {
                //return new BadRequestResult();
                return BadRequest("Book id is not supplied.");
            }

            //Book id can't be empty
            if (string.IsNullOrEmpty(Convert.ToString(Request.Query["bookid"])))
            {
                return BadRequest("Book id can't be null or empty.");
            }

            //Book id should be between 1 to 100
            int bookId = Convert.ToInt32(ControllerContext.HttpContext.Request.Query["bookid"]);
            if (bookId <= 0)
            {
                return BadRequest("Book id can't be less than or equal to zero.");
            }

            if (bookId > 1000)
            {
                return NotFound("Book id can't be greater than 1000");
            }

            //isLoggedin should be true
            if (!Convert.ToBoolean(Request.Query["isLoggedin"]))
            {
                //return Unauthorized("User must be authenticated");
                return StatusCode(401);
            }

            return File("/dummy.pdf", "application/pdf");
        }
    }
}
