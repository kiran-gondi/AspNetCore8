using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers
{
    //[Route("persons")]
    [Route("[controller]")]
    public class PersonsController : Controller
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;

        public PersonsController(IPersonService personService, ICountriesService countriesService)
        {
            _personService = personService;
            _countriesService = countriesService;
        }

        //URL: persons/index
        //[Route("index")]
        [Route("[action]")]
        [Route("/")]
        public IActionResult Index(string searchBy, string? searchString, 
            string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(PersonResponse.PersonName), "Person Name" },
                { nameof(PersonResponse.Email), "Email" },
                { nameof(PersonResponse.DateOfBirth), "Date Of Birth" },
                { nameof(PersonResponse.Gender), "Gender" },
                { nameof(PersonResponse.CountryID), "Country" },
                { nameof(PersonResponse.Address), "Address" },
            };
            //List<PersonResponse> persons = _personService.GetAllPersons();
            List<PersonResponse> persons = _personService.GetFilteredPersons(searchBy, searchString);
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            //Sorting
            List<PersonResponse> sortedPersons = _personService.GetSortedPersons(persons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();

            return View(sortedPersons);
        }

        //Executes when the user clicks on "Create Person" hyperlink (while opening the create view)
        //[Route("create")]
        [Route("[action]")]
        //URL: persons/create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.AllCountries = _countriesService.GetAllCountries();
            return View();
        }

        [HttpPost]
        [Route("persons/create")]
        public IActionResult Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AllCountries = _countriesService.GetAllCountries();

                ViewBag.Errors = ModelState.Values.SelectMany(x=>x.Errors).Select(e=>e.ErrorMessage).ToList();
                return View();
            }

            PersonResponse personResponse = _personService.AddPerson(personAddRequest);
            return RedirectToAction("Index", "Persons");
        }
    }
}
