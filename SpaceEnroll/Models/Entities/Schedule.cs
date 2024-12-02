using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceEnroll.Models.Entities
{
    public class Schedule
    {
        [Key]
        [Required]
        public string SubjectCode { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public string Days { get; set; }

        [Required]
        public string Section { get; set; }

        [Required]
        public string Room { get; set; }

        [Required]
        public string SchoolYear { get; set; }

    }
}
