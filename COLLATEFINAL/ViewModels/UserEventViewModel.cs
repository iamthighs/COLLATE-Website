using System.ComponentModel.DataAnnotations;

namespace COLLATEFINAL.ViewModels
{
    public class UserEventViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsSelected { get; set; }
    }
}
