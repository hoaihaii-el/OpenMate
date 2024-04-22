using System.ComponentModel.DataAnnotations;

namespace StaffManagmentNET.Models
{
    public class ChiTietCongViec
    {
        [Key, MaxLength(50)]
        public string NgayCC { get; set; } = DateTime.Now.ToShortDateString();
        [Key, MaxLength(10)]
        public string MaNhanVien { get; set; } = "";
        [Key]
        public int ThuTu { get; set; }
        public string TenCongViec { get; set; } = "";
        [MaxLength(50)]
        public string TrangThai { get; set; } = "";
        public DateTime BatDau { get; set; }
        public DateTime KetThuc { get; set; }
    }
}
