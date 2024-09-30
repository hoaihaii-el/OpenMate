using System.ComponentModel.DataAnnotations;

namespace StaffManagmentNET.Responses
{
    public class StaffResponse
    {
        public string StaffID { get; set; } = "";
        public string StaffName { get; set; } = "";
        public string Gender { get; set; } = "";
        public string DateBirth { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public string StartWork { get; set; } = "";
        public string StartFullTime { get; set; } = "";
        public string Title { get; set; } = "";
        public string Level { get; set; } = "";
        public string CompanyEmail { get; set; } = "";
        public string PersonalEmail { get; set; } = "";
        public string AvatarURL { get; set; } = "";
        public string ManagerID { get; set; } = "";
        public string ManagerName { get; set; } = "";
        public string DivisionID { get; set; } = "";
        public string AllDivision { get; set; } = "";
        public string Roles { get; set; } = "";
        public string BHXH { get; set; } = "";
        public string BHYT { get; set; } = "";
        public string BHTN { get; set; } = "";
        public string MaSoThue { get; set; } = "";
        public string HDLD { get; set; } = "";
        public string SoHDLD { get; set; } = "";
        public string BankAccount { get; set; } = "";
        public string BankName { get; set; } = "";
        public string Degree { get; set; } = "";
        public string Cert { get; set; } = "";
    }
}
