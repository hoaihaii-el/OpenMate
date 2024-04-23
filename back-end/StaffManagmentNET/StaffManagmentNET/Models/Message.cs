using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaffManagmentNET.Models
{
    public class Message
    {
        [Key, MaxLength(50)]
        public string MessageID { get; set; } = "";
        [MaxLength(50)]
        public string RoomID { get; set; } = "";
        public string Content { get; set; } = "";
        public string MessageType { get; set; } = "";
        public string ResourceURL { get; set; } = "";
        [MaxLength(10)]
        public string SenderID { get; set; } = "";
        public DateTime SendTime { get; set; }

        [ForeignKey("RoomID")]
        public ChatRoom? ChatRoom { get; set; }
        [ForeignKey("SenderID")]
        public Staff? Sender { get; set; }
    }
}
