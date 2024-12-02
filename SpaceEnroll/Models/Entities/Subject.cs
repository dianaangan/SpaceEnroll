using System.ComponentModel.DataAnnotations;

namespace SpaceEnroll.Models.Entities
{
    public class Subject
    {
        [Key]
        [Required]
        public string SubjectCode { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public int Units { get; set; }

        [Required]
        public string Offering { get; set; }

        [Required]
        public string CourseCode { get; set; }

        [Required]
        public string CurriculumYear { get; set; }
    }
}