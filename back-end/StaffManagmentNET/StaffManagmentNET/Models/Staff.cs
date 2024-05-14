using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaffManagmentNET.Models
{
    public class Staff
    {
        [Key, MaxLength(10)]
        public string StaffID { get; set; } = "";
        [MaxLength(100)]
        public string StaffName { get; set; } = "";
        public DateTime StartWork { get; set; }
        public DateTime StartFullTime { get; set; }
        public string Title { get; set; } = "";
        public string Level { get; set; } = "";

        [MaxLength(50)]
        public string CompanyEmail { get; set; } = "";

        [MaxLength(20)]
        public string Phone { get; set; } = "";
        public bool Male { get; set; } = false;
        public string Address { get; set; } = "";
        public DateTime DateBirth { get; set; }

        [MaxLength(20)]
        public string PersonalEmail { get; set; } = "";
        public string AvatarURL { get; set; } = "";


        [MaxLength(10)]
        public string ManagerID { get; set; } = "";
        [MaxLength(50)]
        public string DivisionID { get; set; } = "";

        [ForeignKey("ManagerID")]
        public Staff? Manager { get; set; }
        [ForeignKey("DivisionID")]
        public Division? Division { get; set; }
    }
}
