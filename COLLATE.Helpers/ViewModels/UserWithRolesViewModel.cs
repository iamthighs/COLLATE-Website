using Microsoft.AspNetCore.Identity;
using COLLATE.Helpers.Data;

namespace COLLATE.Helpers.ViewModels
{
    public class UserWithRolesViewModel
    {
        public AppIdentityUser User { get; set; }
        public IList<string> Roles { get; set; }
    }


}
