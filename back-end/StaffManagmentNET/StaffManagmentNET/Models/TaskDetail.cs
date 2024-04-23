using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaffManagmentNET.Models
{
    public class TaskDetail
    {
        [Key, MaxLength(50)]
        public string Date { get; set; } = DateTime.Now.ToShortDateString();
        [Key, MaxLength(10)]
        public string StaffID { get; set; } = "";
        [Key]
        public int Order { get; set; }
        public string TaskName { get; set; } = "";
        [MaxLength(50)]
        public string Status { get; set; } = "";
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [ForeignKey("StaffID")]
        public Staff? Staff { get; set; }
    }
}
