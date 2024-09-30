using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaffManagmentNET.Models
{
    public class Notification
    {
        [Key, MaxLength(50)]
        public string NotiID { get; set; } = "";
        public string NotiName { get; set; } = "";
        public string Content { get; set; } = "";
        [MaxLength(50)]
        public string Level { get; set; } = "";
        public DateTime Date { get; set; }
    }
}
