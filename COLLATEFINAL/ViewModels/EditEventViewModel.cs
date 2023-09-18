using COLLATEFINAL.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace COLLATEFINAL.ViewModels
{
    public class EditEventViewModel
    {
        public EditEventViewModel()
        {
            Users = new List<string>();
        }
        public int Id { get; set; }
        public string EventTitle { get; set; }
        public List<string> Users { get; set; }
    }
}
