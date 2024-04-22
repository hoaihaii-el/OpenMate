using System.ComponentModel.DataAnnotations;

namespace StaffManagmentNET.Models
{
    public class NhanVien
    {
        [Key, MaxLength(10)]
        public string MaNV { get; set; } = "";
        [MaxLength(100)]
        public string HoTen { get; set; } = "";
        public DateTime NgayVaoLam { get; set; }
        public DateTime NgayFulltime { get; set; }
        public string ChucVu { get; set; } = "";
        public string Level { get; set; } = "";

        [MaxLength(50)]
        public string EmailCongTy { get; set; } = "";

        [MaxLength(20)]
        public string SoDienThoai { get; set; } = "";
        public bool Male { get; set; } = false;
        public string DiaChi { get; set; } = "";
        public DateTime NgaySinh { get; set; }

        [MaxLength(20)]
        public string EmailCaNhan { get; set; } = "";



        [MaxLength(10)]
        public string MaNguoiQuanLy { get; set; } = "";
        [MaxLength(50)]
        public string MaPhongBan { get; set; } = "";
    }
}
