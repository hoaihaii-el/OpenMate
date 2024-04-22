using System.ComponentModel.DataAnnotations;

namespace StaffManagmentNET.Models
{
    public class ThietBi
    {
        [Key, MaxLength(50)]
        public string MaThietBi { get; set; } = "";
        public string TenThietBi { get; set; } = "";
        [MaxLength(50)]
        public string LoaiThietBi { get; set; } = "";
        [MaxLength(10)]
        public string MaNhanVien { get; set; } = "";
    }
}
