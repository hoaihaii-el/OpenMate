using System.ComponentModel.DataAnnotations;

namespace StaffManagmentNET.Models
{
    public class BangChamCong
    {
        [Key, MaxLength(50)]
        public string NgayCC { get; set; } = DateTime.Now.ToShortDateString();
        [Key, MaxLength(10)]
        public string MaNhanVien { get; set; } = "";
        public DateTime BatDau { get; set; }
        public DateTime KetThuc { get; set; }
        public int SoGioNghiTrua { get; set; } = 1;
        public string LoaiHinhLamViec { get; set; } = "";
    }
}
