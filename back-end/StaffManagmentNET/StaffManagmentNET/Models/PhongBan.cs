using System.ComponentModel.DataAnnotations;

namespace StaffManagmentNET.Models
{
    public class PhongBan
    {
        [Key, MaxLength(50)]
        public string MaPhongBan { get; set; } = "";
        public string TenPhongBan { get; set; } = "";
        public string MaTruongPhong { get; set; } = "";
    }
}
