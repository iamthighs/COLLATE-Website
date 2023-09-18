using COLLATEFINAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COLLATEFINAL.Data
{
    public class AppIdentityUser: IdentityUser
    {

        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        public string ImageUrl { get; set; }

        [Display(Name = "Profile Picture")]
        [NotMapped]
        public IFormFile? CoverImage { get; set; }
        public ICollection<EventsModel> EventsAttendance { get; set; } = new List<EventsModel>();
    }

   
    public enum Roles
    {
        Administrator,
        Student,
        Faculty,
        [Display(Name = "SCENE Officer")]
        sceneOfficer
    }

    public class SearchViewModel
    {
        public List<GameAndWebDevModel> ResultsModel1 { get; set; }
        public List<ResearchPapersModel> ResultsModel2 { get; set; }
        public List<EventsModel> ResultsModel3 { get; set; }
        public List<SubjectModel> ResultsModel4 { get; set; }
    }


}
