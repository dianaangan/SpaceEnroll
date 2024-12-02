﻿using System.ComponentModel.DataAnnotations;

namespace SpaceEnroll.Models
{
    public class LoginCoderViewModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
