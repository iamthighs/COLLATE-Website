using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace COLLATE.Helpers.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }


    
}
