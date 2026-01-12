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
    public class EventsCreateDto
    {
        [Required]
        public string Title { get; set; } = default!;

        [Required]
        public string Description { get; set; } = default!;

        [Required]
        public string Category { get; set; } = default!;

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public IFormFile CoverImage { get; set; } = default!;
    }

    public class EventsUpdateDto
    {
        [Required]
        public string Title { get; set; } = default!;

        [Required]
        public string Description { get; set; } = default!;

        [Required]
        public string Category { get; set; } = default!;

        [Required]
        public DateTime EventDate { get; set; }

        // Optional on update
        public IFormFile? CoverImage { get; set; }
    }

    public class EventDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public List<AttendeeDto> Attendees { get; set; }
    }

    public class AttendeeDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }

}
