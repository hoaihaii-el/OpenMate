using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaffManagmentNET.Models
{
    public class FamilyInfo
    {
        [Key, MaxLength(10)]
        public string StaffID { get; set; } = "";
        [Key]
        public int Order { get; set; }
        public string FullName { get; set; } = "";
        public string Relationship { get; set; } = "";
        public string Phone { get; set; } = "";
        public int YearBirth { get; set; }

        [ForeignKey("StaffID")]
        public Staff? Staff { get; set; }
    }
}
