using Microsoft.AspNetCore.Mvc;
using Assignment10.Models;

namespace Assignment10.Controllers
{
    public class BankController : Controller
    {
        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            return Content("Welcome to the Best Bank");
        }

        [HttpGet]
        [Route("/account-details")]
        public IActionResult Account()
        {
            Account account = new Account() { AccountNumber = 1001, AccountHolderName = "Bob", CurrentBalance = 5000 };
            return Json(account);
        }

        [HttpGet]
        [Route("/account-statement")]
        public IActionResult AccountStatement()
        {
            return File("/dummy-pdf_2.pdf", "application/pdf");
        }

        [HttpGet]
        [Route("/get-current-balance/{accountNumber:int?}")]
        public IActionResult AccountBalance()
        {
            Account account = new Account() { AccountNumber = 1001, AccountHolderName = "Bob", CurrentBalance = 5000 };
            int accountNumber = Convert.ToInt32(Request.RouteValues["accountNumber"]);

            if(accountNumber == 0)
            {
                return NotFound("Account Number should be supplied");
            }

            if (account.AccountNumber == accountNumber)
            {
                return Ok(account.CurrentBalance);
            }
            else
            {
                return BadRequest($"Account Number should be {account.AccountNumber}");
            }
        }
    }
}
