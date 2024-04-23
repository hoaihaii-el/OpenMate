using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaffManagmentNET.Models
{
    public class Division
    {
        [Key, MaxLength(50)]
        public string DivisionID { get; set; } = "";
        public string DivisionName { get; set; } = "";
        [MaxLength(10)]
        public string ManagerID { get; set; } = "";
    }
}
