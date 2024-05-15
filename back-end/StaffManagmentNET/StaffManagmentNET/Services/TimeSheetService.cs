using Microsoft.EntityFrameworkCore;
using StaffManagmentNET.Models;
using StaffManagmentNET.Repositories;
using System.Globalization;

namespace StaffManagmentNET.Services
{
    public class TimeSheetService : ITimeSheetRepo
    {
        private readonly DataContext _context;

        public TimeSheetService(DataContext context)
        {
            _context = context;
        }

        public async Task CheckIn(string staffID)
        {
            var timeSheet = await _context.TimeSheets.FindAsync(DateTime.Now.ToShortDateString(), staffID);

            if (timeSheet != null)
            {
                throw new Exception("Already check-in");
            }

            _context.TimeSheets.Add(new TimeSheet
            {
                Date = DateTime.Now.ToShortDateString(),
                StaffID = staffID,
                CheckIn = DateTime.Now
            });

            await _context.SaveChangesAsync();
        }

        public async Task CheckOut(string staffID)
        {
            var timeSheet = await _context.TimeSheets.FindAsync(DateTime.Now.ToShortDateString(), staffID);

            if (timeSheet == null)  
            {
                throw new Exception("You haven't check-in yet");
            }

            timeSheet.CheckOut = DateTime.Now;

            var total = (timeSheet.CheckOut - timeSheet.CheckIn).TotalHours;

            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                timeSheet.Total = total * 1.5;
            }

            if (IsVietNameseHoliday())
            {
                timeSheet.Total = total * 2;
            }

            _context.TimeSheets.Update(timeSheet);
            await _context.SaveChangesAsync();
        }

        public async Task ReCheckIn(string staffID, DateTime datetime)
        {
            var timeSheet = await _context.TimeSheets.FindAsync(datetime.ToShortDateString(), staffID);

            if (timeSheet == null)
            {
                throw new Exception("No data yet");
            }

            if (datetime.Date != timeSheet.CheckOut.Date || datetime > timeSheet.CheckOut)
            {
                throw new Exception("Check-in data invalid!");
            }

            timeSheet.CheckIn = datetime;
            _context.TimeSheets.Update(timeSheet);
            await _context.SaveChangesAsync();
        }

        public async Task ReCheckOut(string staffID, DateTime datetime)
        {
            var timeSheet = await _context.TimeSheets.FindAsync(datetime.ToShortDateString(), staffID);

            if (timeSheet == null)
            {
                throw new Exception("No data yet");
            }

            if (datetime.Date != timeSheet.CheckIn.Date || datetime < timeSheet.CheckIn)
            {
                throw new Exception("Check-out data invalid!");
            }

            timeSheet.CheckOut = datetime;
            _context.TimeSheets.Update(timeSheet);
            await _context.SaveChangesAsync();
        }

        public async Task<double> GetAvgByMonth(string staffID, int month, int year)
        {
            var totalThisMonth = await _context.TimeSheets
                .Where(t => t.StaffID == staffID && t.CheckIn.Month == month && t.CheckIn.Year == year)
                .SumAsync(t => t.Total);

            return totalThisMonth / DateTime.DaysInMonth(month, year);
        }

        public async Task<double> GetTotalByMonth(string staffID, int month, int year)
        {
            var totalThisMonth = await _context.TimeSheets
                .Where(t => t.StaffID == staffID && t.CheckIn.Month == month && t.CheckIn.Year == year)
                .SumAsync(t => t.Total);

            return totalThisMonth;
        }

        public bool IsVietNameseHoliday()
        {
            var date = DateTime.Now.Date;

            List<DateTime> holidays = new List<DateTime>
            {
                new DateTime(date.Year, 1, 1),    // Tết Dương lịch
                new DateTime(date.Year, 4, 30),   // Ngày Giải phóng miền Nam
                new DateTime(date.Year, 5, 1),    // Ngày Quốc tế Lao động
                new DateTime(date.Year, 9, 2)     // Ngày Quốc khánh
            };

            var lunarCalendar = new ChineseLunisolarCalendar();
            holidays.Add(lunarCalendar.ToDateTime(date.Year, 3, 10, 6, 0, 0, 0));
            holidays.Add(lunarCalendar.ToDateTime(date.Year, 1, 1, 6, 0, 0, 0));
            holidays.Add(lunarCalendar.ToDateTime(date.Year, 1, 2, 6, 0, 0, 0));
            holidays.Add(lunarCalendar.ToDateTime(date.Year, 1, 3, 6, 0, 0, 0));

            foreach (var holiday in holidays)
            {
                if (date.Date == holiday.Date) return true;
            }

            return false;
        }
    }
}
