using System.ComponentModel.DataAnnotations;

namespace SpaceEnroll.Models
{
    public class AddSubjectViewModel
    {
        public string SubjectCode { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Units { get; set; }
        public string Offering { get; set; }
        public string CourseCode { get; set; }
        public string CurriculumYear { get; set; }
    }
}
