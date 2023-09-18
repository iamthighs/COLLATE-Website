using COLLATEFINAL.Models;
using System.ComponentModel.DataAnnotations;

namespace COLLATEFINAL.ViewModels
{
    public class EditSubjViewModel
    {
        public EditSubjViewModel()
        {
            Lectures = new List<LectureModel>();
            Videos = new List<VideosModel>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Subject is required")]
        public string Subject { get; set; }
        public IList<LectureModel> Lectures { get; set; }
        public IList<VideosModel> Videos { get; set; }
        public IList<EditSubjViewModel> EditSubj { get; set; }
        public bool IsSelected { get; set; }

    }
    
}
