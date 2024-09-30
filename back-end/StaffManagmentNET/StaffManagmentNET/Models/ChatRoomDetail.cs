using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaffManagmentNET.Models
{
    public class ChatRoomDetail
    {
        [Key, MaxLength(50)]
        public string RoomID { get; set; } = "";
        [Key, MaxLength(10)]
        public string StaffID { get; set; } = "";

        [ForeignKey("RoomID")]
        public ChatRoom? ChatRoom { get; set; }
        [ForeignKey("StaffID")]
        public Staff? Staff { get; set; }
    }
}
