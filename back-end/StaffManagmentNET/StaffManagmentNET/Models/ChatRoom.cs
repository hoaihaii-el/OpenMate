using System.ComponentModel.DataAnnotations;

namespace StaffManagmentNET.Models
{
    public class ChatRoom
    {
        [Key, MaxLength(50)]
        public string RoomID { get; set; } = "";
        public string RoomName { get; set; } = "";
    }
}
