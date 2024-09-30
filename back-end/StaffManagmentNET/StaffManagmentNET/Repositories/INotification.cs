using StaffManagmentNET.Responses;
using StaffManagmentNET.ViewModels;

namespace StaffManagmentNET.Repositories
{
    public interface INotification
    {
        Task<IEnumerable<NotificationResponse>> GetAllNoti();
        Task NewNotification(NotificationVM vm);
    }
}
