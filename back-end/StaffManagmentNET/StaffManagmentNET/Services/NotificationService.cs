using Microsoft.EntityFrameworkCore;
using StaffManagmentNET.Models;
using StaffManagmentNET.Repositories;
using StaffManagmentNET.Responses;
using StaffManagmentNET.ViewModels;

namespace StaffManagmentNET.Services
{
    public class NotificationService : INotification
    {
        private readonly DataContext _context;
        public NotificationService(DataContext context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<NotificationResponse>> GetAllNoti()
        {
            var notis = new List<NotificationResponse>();

            var notisFromDB = await _context.Notifications.OrderByDescending(n => n.Date).ToListAsync();

            foreach (var noti in notisFromDB)
            {
                notis.Add(new NotificationResponse
                {
                    NotiName = noti.NotiName,
                    NotiID = noti.NotiID,
                    Content = noti.Content,
                    Level = noti.Level,
                    Date = noti.Date.ToString("dd/MM/yyyy")
                });
            }

            return notis;
        }

        public async Task NewNotification(NotificationVM vm)
        {
            _context.Notifications.Add(new Notification
            {
                NotiID = Guid.NewGuid().ToString(),
                NotiName = vm.NotiName,
                Content = vm.Content,
                Level = vm.Level,
                Date = DateTime.Now
            });

            await _context.SaveChangesAsync();
        }
    }
}
