using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;


namespace COLLATEFINAL.Models
{
    public class PrototypeModel
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Subject")]
        public string? Header { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        [Display(Name = "Professor")]
        public string? Authors { get; set; }
        [Required]
        [Display(Name = "Year & Section")]
        public string? YearSec { get; set; }
        [Required]
        [Display(Name = "Date Published")]
        public DateTime PostedDate { get; set; }
        [Required]
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? FileUrl { get; set; }
        [Display(Name = "Cover Image")]
        [NotMapped]
        public IFormFile? CoverImage { get; set; }
        [Display(Name = "Upload your PDF File")]
        [NotMapped]
        public IFormFile? UploadedCoverImage { get; set; }
        

    }
}
