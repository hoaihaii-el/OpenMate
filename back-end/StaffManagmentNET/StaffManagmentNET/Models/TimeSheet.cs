using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaffManagmentNET.Models
{
    public class TimeSheet
    {
        [Key, MaxLength(50)]
        public string Date { get; set; } = DateTime.Now.ToShortDateString();
        [Key, MaxLength(10)]
        public string StaffID { get; set; } = "";
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int LunchBreakHour { get; set; } = 1;
        public string WorkingType { get; set; } = "";

        [ForeignKey("StaffID")]
        public Staff? Staff { get; set; }
    }
}
