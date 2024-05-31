using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StaffManagmentNET.Models;

namespace StaffManagmentNET.Repositories
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Staff> Staffs { get; set; }
        public DbSet<FamilyInfo> FamilyInfoes { get; set; }
        public DbSet<TimeSheet> TimeSheets { get; set; }
        public DbSet<TaskDetail> TaskDetails { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<ChatRoomDetail> ChatRoomsDetail { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestCreateDetail> RequestCreateDetails { get; set; }
        public DbSet<RequestAcceptDetail> RequestAcceptDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<FamilyInfo>().HasKey(f => new { f.StaffID, f.Order });
            builder.Entity<TimeSheet>().HasKey(t => new { t.Date, t.StaffID });
            builder.Entity<TaskDetail>().HasKey(t => new { t.Date, t.StaffID, t.Order });
            builder.Entity<ChatRoomDetail>().HasKey(r => new { r.StaffID, r.RoomID });
            builder.Entity<RequestAcceptDetail>().HasKey(r => new { r.CreateID, r.ManagerID });

            base.OnModelCreating(builder);
        }
    }
}
