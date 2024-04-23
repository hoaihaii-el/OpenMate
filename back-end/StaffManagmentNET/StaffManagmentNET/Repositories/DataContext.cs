using Microsoft.EntityFrameworkCore;
using StaffManagmentNET.Models;

namespace StaffManagmentNET.Repositories
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        DbSet<Staff>? Staffs { get; set; }
        DbSet<FamilyInfo>? FamilyInfoes { get; set; }
        DbSet<TimeSheet>? TimeSheets { get; set; }
        DbSet<TaskDetail>? TaskDetails { get; set; }
        DbSet<Device>? Devices { get; set; }
        DbSet<Division>? Divisions { get; set; }
        DbSet<Notification>? Notifications { get; set; }
        DbSet<ChatRoom>? ChatRooms { get; set; }
        DbSet<ChatRoomDetail>? ChatRoomsDetail { get; set; }
        DbSet<Message>? Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<FamilyInfo>().HasKey(f => new { f.StaffID, f.Order });
            builder.Entity<TimeSheet>().HasKey(t => new { t.Date, t.StaffID });
            builder.Entity<TaskDetail>().HasKey(t => new { t.Date, t.StaffID });
            builder.Entity<ChatRoomDetail>().HasKey(r => new { r.StaffID, r.RoomID });

            base.OnModelCreating(builder);
        }
    }
}
