using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using COLLATEFINAL.Data;
using COLLATEFINAL.ViewModels;
using Microsoft.AspNetCore.Http;


namespace COLLATEFINAL.Models
{
    public class EventsModel
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Category { get; set; }
        [Required]
        public string? Title { get; set; }
        
        [Required]
        public string? Objectives { get; set; }
        [Required]
        [Display(Name = "Date Conducted")]
        public DateTime PostedDate { get; set; }
        [Required]
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }

        [Required]
        public string? IFrame { get; set; }

        [Display(Name = "Official Poster")]
        [NotMapped]
        public IFormFile? CoverImage { get; set; }
        [Display(Name = "Status")]
        public bool IsDone { get; set; }
        [Display(Name = "Attendees")]
        public ICollection<AppIdentityUser> Attendees { get; set; } = new List<AppIdentityUser>();
    }

    
}
