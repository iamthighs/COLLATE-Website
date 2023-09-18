using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;


namespace COLLATEFINAL.Models
{
    public class GameAndWebDevModel
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Team Name")]
        public string GroupName { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Developers")]
        public string DevelopersName { get; set; }
        [Required]
        [Display(Name = "Year & Section")]
        public string YearSec { get; set; }
        [Required]
        [Display(Name = "Date Published")]
        public DateTime PostedDate { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Video Link")]
        public string VidLink { get; set; }
        [Required]
        [Display(Name = "Game Link")]
        public string GameLink { get; set; }
        
        public string ImageUrl { get; set; }
        [Display(Name = "Cover Image")]
        [NotMapped]
        public IFormFile? CoverImage { get; set; }
        public List<Like> Likes { get; set; }
        public List<Comment> Comments { get; set; }

    }
}
