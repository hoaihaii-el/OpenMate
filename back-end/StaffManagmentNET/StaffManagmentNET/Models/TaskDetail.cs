using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaffManagmentNET.Models
{
    public class TaskDetail
    {
        [Key, MaxLength(50)]
        public string Date { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        [Key, MaxLength(10)]
        public string StaffID { get; set; } = "";
        [Key]
        public int Order { get; set; }
        public string TaskName { get; set; } = "";
        [MaxLength(30)]
        public string Status { get; set; } = "";
        public double Estimate { get; set; } = 1;
        public string Note { get; set; } = "";
        public double Evaluate { get; set; }
        public string Feedback { get; set; } = "";
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [ForeignKey("StaffID")]
        public Staff? Staff { get; set; }
    }
}
