using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaffManagmentNET.Models
{
    public class RequestCreateDetail
    {
        [Key, MaxLength(50)]
        public string DetailID { get; set; } = "";
        [MaxLength(50)]
        public string RequestID { get; set; } = "";
        [MaxLength(10)]
        public string StaffID { get; set; } = "";
        public DateTime CreateTime { get; set; }
        public string Content1 { get; set; } = "";
        public string Content2 { get; set; } = "";
        public string Content3 { get; set; } = "";
        public string Evidence { get; set; } = "";
        public int CurrentLevel { get; set; }
        public string Status { get; set; } = "";

        [ForeignKey("RequestID")]
        public Request? Request { get; set; }
        [ForeignKey("StaffID")]
        public Staff? Staff { get; set; }
    }
}
