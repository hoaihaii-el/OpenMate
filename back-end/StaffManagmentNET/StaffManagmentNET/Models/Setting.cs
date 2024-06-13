using System.ComponentModel.DataAnnotations;

namespace StaffManagmentNET.Models
{
    public class Setting
    {
        [Key, MaxLength(255)]
        public string Key { get; set; } = "";
        public string Value { get; set; } = "";
        public string Type { get; set; } = "";
    }
}
