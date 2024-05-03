using System.ComponentModel.DataAnnotations;

namespace StaffManagmentNET.Models
{
    public class Request
    {
        [Key, MaxLength(50)]
        public string RequestID { get; set; } = "";
        public string RequestType { get; set; } = "";
        public string RequestName { get; set; } = "";
        public string Title1 { get; set; } = "";
        public string Title2 { get; set; } = "";
        public string Title3 { get; set; } = "";
        public int AcceptLevel { get; set; }
        [MaxLength(10)]
        public string ExternalAcceptID { get; set; } = "";
    }
}
