using Microsoft.AspNetCore.Mvc;
using ViewComponentsExample.Models;

namespace ViewComponentsExample.ViewComponents
{
    public class GridViewComponent : ViewComponent
    {
       public async Task<IViewComponentResult> InvokeAsync()
        {
            //return View(); //invoked a partial view Views/Shared/Components/Grid/Default.cshtml

            PersonGridModel personGridModel = new()
            {
                GridTitle = "Person list",
                Persons = new List<Person>
                {
                    new Person { PersonName = "Tom", JobTitle = "Junior" },
                    new Person { PersonName = "Beam", JobTitle = "Senior" },
                    new Person { PersonName = "Jim", JobTitle = "Expert" }
                }
            };
            //ViewData["Grid"] = personGridModel;
            //ViewBag.Grid = personGridModel;
            return View("Default1", personGridModel);
        }
    }
}
