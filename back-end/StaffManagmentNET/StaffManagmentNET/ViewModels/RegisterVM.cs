using StaffManagmentNET.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StaffManagmentNET.ViewModels
{
    public class RegisterVM
    {
        public string FullName { get; set; } = "";
        public string Title { get; set; } = "";
        public string Level { get; set; } = "";
        public string Phone { get; set; } = "";
        public bool Male { get; set; } = false;
        public string Address { get; set; } = "";
        public DateTime DateBirth { get; set; }
        public string PersonalEmail { get; set; } = "";
        public string Password { get; set; } = "";
        public string ManagerID { get; set; } = "";
        public string DivisionID { get; set; } = "";
        public string Roles { get; set; } = "";
    }
}
