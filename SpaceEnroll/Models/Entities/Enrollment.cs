﻿using System.ComponentModel.DataAnnotations;

namespace SpaceEnroll.Models.Entities
{
    public class Enrollment
    {
        [Key]
        [Required]
        public string StudentId { get; set; }

        [Required]
        public string SubjectCode { get; set; }

        [Required]
        public DateTime DateEnrolled { get; set; }

        [Required]
        public string Encoder { get; set; }

    }
}