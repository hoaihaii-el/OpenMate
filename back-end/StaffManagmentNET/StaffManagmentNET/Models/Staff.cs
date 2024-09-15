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
        public string BHXH { get; set; } = "";
        public string BHYT { get; set; } = "";
        public string BHTN { get; set; } = "";
        public string MaSoThue { get; set; } = "";
        public string HDLD { get; set; } = "";
        public string SoHDLD { get; set; } = "";
        public string BankAccount { get; set; } = "";
        public string BankName { get; set; } = "";


        [MaxLength(10)]
        public string ManagerID { get; set; } = "";
        [MaxLength(50)]
        public string DivisionID { get; set; } = "";

        [ForeignKey("DivisionID")]
        public Division? Division { get; set; }
    }
}
