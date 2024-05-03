using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaffManagmentNET.Models
{
    public class RequestAcceptDetail
    {
        [Key, MaxLength(50)]
        public string DetailID { get; set; } = "";
        [Key, MaxLength(10)]
        public string ManagerID { get; set; } = "";
        public DateTime AcceptTime { get; set; }

        [ForeignKey("ManagerID")]
        public Staff? Manager { get; set; }
    }
}
