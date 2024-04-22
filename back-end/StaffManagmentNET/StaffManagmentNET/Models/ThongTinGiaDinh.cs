using System.ComponentModel.DataAnnotations;

namespace StaffManagmentNET.Models
{
    public class ThongTinGiaDinh
    {
        [Key, MaxLength(10)]
        public string MaNhanVien { get; set; } = "";
        [Key]
        public int STT { get; set; }
        public string HoTen { get; set; } = "";
        public string MoiQuanHe { get; set; } = "";
        public string SoDienThoai { get; set; } = "";
        public int NamSinh { get; set; }
    }
}
