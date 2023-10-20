using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using COLLATEFINAL.ViewModels;

namespace COLLATEFINAL.ViewComponents
{
    public class CreateRoleModalViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("Default");
        }
    }
}
