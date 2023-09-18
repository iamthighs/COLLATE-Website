using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;


namespace COLLATEFINAL.Models
{
    public class SubjectModel
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string? Subject { get; set; }

        [Required]
        public string? Code { get; set; }

        [Required]
        [Display(Name = "Date Published")]
        public DateTime PostedDate { get; set; }
        
        public string? ImageUrl { get; set; }
        
        [Display(Name = "Cover Image")]
        [NotMapped]
        public IFormFile? CoverImage { get; set; }
        
        

    }

    public class LectureModel
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Subject { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        [Display(Name = "Date Published")]
        public DateTime PostedDate { get; set; }

        public string? FileUrl { get; set; }

        [Display(Name = "Upload your PDF File")]
        [NotMapped]
        public IFormFile? UploadedPDFFile { get; set; }



    }

    public class VideosModel
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Subject { get; set; }
        [Required]
        public string? Title { get; set; }

        [Required]
        public string? IFrame { get; set; }

        [Required]
        [Display(Name = "Date Published")]
        public DateTime PostedDate { get; set; }

        
    }

    public class FeedbackModel
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string? FullName { get; set; }
        [Required]
        public string? Feedback { get; set; }


    }
}
