using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaffManagmentNET.Models
{
    public class Device
    {
        [Key, MaxLength(50)]
        public string DeviceID { get; set; } = "";
        public string DeviceName { get; set; } = "";
        [MaxLength(50)]
        public string DeviceType { get; set; } = "";
        [MaxLength(10)]
        public string StaffID { get; set; } = "";

        [ForeignKey("StaffID")]
        public Staff? Staff { get; set; }
    }
}
