// Models/Entities/Student.cs
using System.ComponentModel.DataAnnotations;

namespace SpaceEnroll.Models.Entities
{
    public class Student
    {

        [Required]
        public string StudentId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Course { get; set; }

        [Required]
        [Range(1, 4)]
        public int Year { get; set; }
    }
}
