using Microsoft.AspNetCore.Identity;
using COLLATEFINAL.Data;

namespace COLLATEFINAL.ViewModels
{
    public class UserWithRolesViewModel
    {
        public AppIdentityUser User { get; set; }
        public IList<string> Roles { get; set; }
    }


}
