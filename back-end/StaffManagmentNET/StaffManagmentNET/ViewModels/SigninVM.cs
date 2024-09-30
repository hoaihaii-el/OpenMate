﻿using System.ComponentModel.DataAnnotations;

namespace StaffManagmentNET.ViewModels
{
    public class SigninVM
    {
        [Required]
        public string UserID { get; set; } = "";
        [Required]
        public string Password { get; set; } = "";
    }
}
