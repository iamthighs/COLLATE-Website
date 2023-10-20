using COLLATEFINAL.Data;
using COLLATEFINAL.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace COLLATEFINAL.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Claims = new List<string>();
            Users = new List<AppIdentityUser>();
        }
        public string Id { get; set; }
        public List<string> Claims { get; set; }
        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }
        public List<AppIdentityUser> Users { get; set; }
    }
}
