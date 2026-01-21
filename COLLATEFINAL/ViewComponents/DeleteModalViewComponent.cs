using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace COLLATEFINAL.ViewComponents
{
    public class DeleteModalViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("Default");
        }
    }
}
