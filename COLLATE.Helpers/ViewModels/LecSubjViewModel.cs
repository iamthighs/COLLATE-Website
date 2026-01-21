using System.ComponentModel.DataAnnotations;

namespace COLLATE.Helpers.ViewModels
{
    public class LecSubjViewModel
    {
        public string Subject { get; set; }
        public int LecId { get; set; }
        public string Title { get; set; }
        public bool IsSelected { get; set; }
    }
}
