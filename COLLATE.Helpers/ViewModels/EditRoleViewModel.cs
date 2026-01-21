using COLLATE.Helpers.Data;
using COLLATE.Helpers.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace COLLATE.Helpers.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Claims = new List<string>();
            Users = new List<RoleUserDto>();
        }
        public string Id { get; set; }
        public List<string> Claims { get; set; }
        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }
        public List<RoleUserDto> Users { get; set; }
    }

    public class RoleUserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }

}
