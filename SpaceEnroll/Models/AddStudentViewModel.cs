// Models/AddStudentViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace SpaceEnroll.Models
{
    public class AddStudentViewModel
    {

        public string StudentId { get; set; } 
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public string Email { get; set; } 
        public string Course { get; set; } 
        public int Year { get; set; } 
    }
}