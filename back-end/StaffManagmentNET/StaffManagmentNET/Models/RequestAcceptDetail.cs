using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaffManagmentNET.Models
{
    public class RequestAcceptDetail
    {
        [Key, MaxLength(50)]
        public string CreateID { get; set; } = "";
        [Key, MaxLength(10)]
        public string ManagerID { get; set; } = "";
        public DateTime ActionTime { get; set; }
        public string Action { get; set; } = "";
    }
}
