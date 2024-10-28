using Microsoft.AspNetCore.Mvc;

namespace ViewComponentsExample.ViewComponents
{
    public class GridViewComponent : ViewComponent
    {
       public async Task<IViewComponentResult> InvokeAsync()
        {
            //return View(); //invoked a partial view Views/Shared/Components/Grid/Default.cshtml
            return View("Default1");
        }
    }
}
