using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace COLLATEFINAL.ViewComponents
{
    public class ManageRoleModalViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("Default");
        }
    }
}
