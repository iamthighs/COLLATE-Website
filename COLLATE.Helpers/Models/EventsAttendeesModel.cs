using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using COLLATE.Helpers.ViewModels;
using Microsoft.AspNetCore.Http;


namespace COLLATE.Helpers.Models
{
    public class EventsAttendeesModel
    {
        [Key]
        [Required]
        public string UserId { get; set; }
        [Key]
        [Required]
        public int EventsId { get; set; }
        [Display(Name = "Status")]
        public bool IsAttended { get; set; }


    }
}
