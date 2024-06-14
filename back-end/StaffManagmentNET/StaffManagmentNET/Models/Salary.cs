using System.ComponentModel.DataAnnotations;

namespace StaffManagmentNET.Models
{
    public class Salary
    {
        [Key]
        public string Key { get; set; } = "";
        [Key]
        public string StaffID { get; set; } = "";
        public string StaffName { get; set; } = "";
        public double TotalHour { get; set; }
        public string Title { get; set; } = "";
        public string TitleAllowance { get; set; } = "0";
        public string DegreeAllowance { get; set; } = "0";
        public string CertAllowance { get; set; } = "0";
        public string GeneralAllowance { get; set; } = "0";
        public string Reward { get; set; } = "0";
        public double Evaluate { get; set; }
        public double Total { get; set; } = 0;
    }
}
