using StaffManagmentNET.Models;
using StaffManagmentNET.Responses;
using StaffManagmentNET.ViewModels;

namespace StaffManagmentNET.Repositories
{
    public interface IChatRepo
    {
        Task AddNewRoom(string users);
        Task<IEnumerable<MessageResponse>> GetAllMessage(string roomID);
        Task<IEnumerable<ChatRoom>> GetAllRoom(string staffID);
        Task ChangeRoomName(string roomID, string newName);
        Task<IEnumerable<ChatRoomDetail>> GetRoomDetail(string roomID);
        Task SendMessage(MessageVM vm);
    }
}
