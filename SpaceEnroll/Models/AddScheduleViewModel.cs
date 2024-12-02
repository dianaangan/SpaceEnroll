using SpaceEnroll.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace SpaceEnroll.Models
{
    public class AddScheduleViewModel
    {
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

        public Subject? Subject { get; set; }
    }
}
