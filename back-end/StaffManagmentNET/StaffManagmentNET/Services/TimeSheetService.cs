using Microsoft.EntityFrameworkCore;
using StaffManagmentNET.Models;
using StaffManagmentNET.Repositories;
using StaffManagmentNET.Responses;
using System;
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
            var timeSheet = await _context.TimeSheets.FindAsync(DateTime.Now.ToString("dd/MM/yyyy"), staffID);

            if (timeSheet != null)
            {
                if (timeSheet.CheckIn.Date < DateTime.Now.Date)
                {
                    timeSheet.CheckIn = DateTime.Now;
                    _context.TimeSheets.Update(timeSheet);
                    await _context.SaveChangesAsync();
                    return;
                }
                throw new Exception("Already check-in");
            }

            _context.TimeSheets.Add(new TimeSheet
            {
                Date = DateTime.Now.ToString("dd/MM/yyyy"),
                StaffID = staffID,
                CheckIn = DateTime.Now
            });

            await _context.SaveChangesAsync();
        }

        public async Task CheckOut(string staffID)
        {
            var timeSheet = await _context.TimeSheets.FindAsync(DateTime.Now.ToString("dd/MM/yyyy"), staffID);

            if (timeSheet == null)  
            {
                throw new Exception("You haven't check-in yet");
            }

            if (timeSheet.CheckOut.Date == DateTime.Now.Date)
            {
                throw new Exception("You have already checked-out");
            }

            timeSheet.CheckOut = DateTime.Now;
            timeSheet.Total = CalcTotal(timeSheet);

            _context.TimeSheets.Update(timeSheet);
            await _context.SaveChangesAsync();
        }

        public async Task<TimeSheet> UpdateData(string staffID, string date, int h1, int m1, int h2, int m2, string type, string wrkType, string off)
        {
            var timeSheet = await _context.TimeSheets.FindAsync(date, staffID);

            switch (type)
            {
                case "all":
                    if (h1 == timeSheet!.CheckOut.Hour && m1 > timeSheet.CheckOut.Minute || h1 > timeSheet.CheckOut.Hour)
                    {
                        throw new Exception("Invalid time");
                    }
                    timeSheet.CheckIn = new DateTime(timeSheet.CheckOut.Year, timeSheet.CheckOut.Month, timeSheet.CheckOut.Day, h1, m1, 0);
                    if (h2 == timeSheet!.CheckOut.Hour && m1 < timeSheet.CheckOut.Minute || h2 < timeSheet.CheckOut.Hour)
                    {
                        throw new Exception("Invalid time");
                    }
                    timeSheet.CheckOut = new DateTime(timeSheet.CheckIn.Year, timeSheet.CheckIn.Month, timeSheet.CheckIn.Day, h2, m2, 0);
                    timeSheet.Total = CalcTotal(timeSheet);

                    if (timeSheet == null)
                    {
                        timeSheet = new TimeSheet()
                        {
                            StaffID = staffID,
                            Date = date,
                            WorkingType = wrkType
                        };
                        _context.TimeSheets.Add(timeSheet);
                    }
                    else
                    {
                        timeSheet.WorkingType = wrkType;
                    }

                    if (timeSheet == null)
                    {
                        timeSheet = new TimeSheet()
                        {
                            StaffID = staffID,
                            Date = date,
                            Off = off
                        };
                        _context.TimeSheets.Add(timeSheet);
                    }
                    else
                    {
                        timeSheet.Off = off;
                    }

                    break;
                case "re-check-in":
                    if (h1 == timeSheet!.CheckOut.Hour && m1 > timeSheet.CheckOut.Minute || h1 > timeSheet.CheckOut.Hour)
                    {
                        throw new Exception("Invalid time");
                    }
                    timeSheet.CheckIn = new DateTime(timeSheet.CheckOut.Year, timeSheet.CheckOut.Month, timeSheet.CheckOut.Day, h1, m1, 0);
                    timeSheet.Total = CalcTotal(timeSheet);
                    break;
                case "re-check-out":
                    if (h2 == timeSheet!.CheckOut.Hour && m1 < timeSheet.CheckOut.Minute || h2 < timeSheet.CheckOut.Hour)
                    {
                        throw new Exception("Invalid time");
                    }
                    timeSheet.CheckOut = new DateTime(timeSheet.CheckIn.Year, timeSheet.CheckIn.Month, timeSheet.CheckIn.Day, h2, m2, 0);
                    timeSheet.Total = CalcTotal(timeSheet);
                    break;
                case "working-type":
                    if (timeSheet == null)
                    {
                        timeSheet = new TimeSheet()
                        {
                            StaffID = staffID,
                            Date = date,
                            WorkingType = wrkType
                        };
                        _context.TimeSheets.Add(timeSheet);
                        await _context.SaveChangesAsync();
                        return timeSheet;
                    }
                    else
                    {
                        timeSheet.WorkingType = wrkType;
                    }
                    break;
                case "off":
                    if (timeSheet == null)
                    {
                        timeSheet = new TimeSheet()
                        {
                            StaffID = staffID,
                            Date = date,
                            Off = off
                        };
                        _context.TimeSheets.Add(timeSheet);
                        await _context.SaveChangesAsync();
                        return timeSheet;
                    }
                    else
                    {
                        timeSheet.Off = off;
                    }
                    break;
            }

            _context.TimeSheets.Update(timeSheet!);
            await _context.SaveChangesAsync();

            return timeSheet!;
        }

        public async Task<double> GetAvgByMonth(string staffID, int month, int year)
        {
            var totalThisMonth = await _context.TimeSheets
                .Where(t => t.StaffID == staffID && t.CheckIn.Month == month && t.CheckIn.Year == year)
                .SumAsync(t => t.Total);

            var count = await _context.TimeSheets
                .Where(t => t.StaffID == staffID && t.CheckIn.Month == month && t.CheckIn.Year == year && t.CheckIn.Date == t.CheckOut.Date)
                .CountAsync();

            if (totalThisMonth == 0) return 0.0;

            return Math.Round(totalThisMonth / count, 1);
        }

        public async Task<double> GetTotalByMonth(string staffID, int month, int year)
        {
            var totalThisMonth = await _context.TimeSheets
                .Where(t => t.StaffID == staffID && t.CheckIn.Month == month && t.CheckIn.Year == year)
                .SumAsync(t => t.Total);

            return totalThisMonth;
        }

        public async Task<TimeSheet> GetTimeSheetByDay(string staffID, int day, int month, int year)
        {
            var date = $"{month}/{day}/{year}";
            var timeSheet = await _context.TimeSheets.FindAsync(date, staffID);

            if (timeSheet == null)
            {
                throw new Exception("Not found!");
            }

            return timeSheet;
        }

        public async Task<IEnumerable<TimeSheetResponse>> GetTimeSheetByMonth(string staffID, int month, int year)
        {
            var sheets = await _context.TimeSheets
                .Where(t => t.Date.Contains(year.ToString()) && t.StaffID == staffID)
                .ToListAsync();

            var result = new List<TimeSheetResponse>();

            var startDate = new DateTime(year, month, 1);

            for (DateTime date = startDate; date.Month == startDate.Month; date = date.AddDays(1))
            {
                var timeSheet = new TimeSheetResponse()
                {
                    Date = $"{date.Day}-{date.DayOfWeek.ToString().Substring(0, 3).ToUpper()}",
                    Key = date.ToString("dd/MM/yyyy"),
                    Today = date.Date == DateTime.Now.Date ? 1 : date.Date < DateTime.Now.Date ? 0 : 2,
                };

                if (timeSheet.Date.Length < 6) timeSheet.Date = "0" + timeSheet.Date;

                timeSheet.CheckIn = timeSheet.CheckOut = "__:__";
                if (IsVietNameseHoliday(date.ToString("dd/MM/yyyy")))
                {
                    timeSheet.Off = "";
                    timeSheet.WorkingType = "Holiday";
                    timeSheet.CheckIn = timeSheet.CheckOut = "";
                    timeSheet.IsDayOff = true;
                }

                if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
                {
                    timeSheet.WorkingType = timeSheet.Off = "";
                    timeSheet.CheckIn = timeSheet.CheckOut = "";
                    timeSheet.IsDayOff = true;
                }

                foreach (var sheet in sheets)
                {
                    if (sheet.Date == date.Date.ToString("dd/MM/yyyy"))
                    {
                        if (sheet.CheckIn.Date == date.Date)
                        {
                            timeSheet.CheckIn = $"{sheet.CheckIn.Hour}:{sheet.CheckIn.Minute}";
                            if (sheet.CheckOut > sheet.CheckIn)
                            {
                                timeSheet.CheckOut = $"{sheet.CheckOut.Hour}:{sheet.CheckOut.Minute}";
                            }
                            timeSheet.LunchBreak = sheet.LunchBreakHour.ToString() + "hr";
                            timeSheet.Total = sheet.Total.ToString() + "hrs";
                        }
                        
                        timeSheet.WorkingType = sheet.WorkingType;
                        timeSheet.Off = sheet.Off;
                    }
                }

                if (timeSheet.Today > 0)
                {
                    timeSheet.LunchBreak = timeSheet.Total = "";
                    timeSheet.CheckIn = timeSheet.CheckOut = "";
                }

                result.Add(timeSheet);
            }

            return result;
        }

        public async Task<TimeSheetDetail> GetSheetDetail(string staffID, int month, int year)
        {
            var workdays = DateTime.DaysInMonth(year, month);
            var startDate = new DateTime(year, month, 1);
            for (DateTime date = startDate; date.Month == startDate.Month; date = date.AddDays(1)) 
            {
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || IsVietNameseHoliday(date.ToString("dd/MM/yyyy")))
                {
                    workdays--;
                }
            }

            var WFH = await _context.TimeSheets.Where(t => t.WorkingType == "WFH").CountAsync();
            var total = await GetTotalByMonth(staffID, month, year);
            var avg = await GetAvgByMonth(staffID, month, year);

            return new TimeSheetDetail
            {
                WorkDays = workdays,
                WFH = WFH,
                Total = total,
                Avg = avg
            };
        }

        double CalcTotal(TimeSheet timeSheet)
        {
            timeSheet.CheckOut = DateTime.Now;

            var total = (timeSheet.CheckOut - timeSheet.CheckIn).TotalHours;

            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                return Math.Round(total * 1.5, 1);
            }

            if (IsVietNameseHoliday(timeSheet.Date))
            {
                return Math.Round(total * 2, 1);
            }

            return Math.Round(total, 1);
        }

        bool IsVietNameseHoliday(string input)
        {
            var date = DateTime.ParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture);

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
