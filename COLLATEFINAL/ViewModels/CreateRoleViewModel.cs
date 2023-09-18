using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace COLLATEFINAL.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }


    
}
