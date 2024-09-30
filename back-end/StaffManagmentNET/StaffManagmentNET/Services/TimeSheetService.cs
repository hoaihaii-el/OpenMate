using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using StaffManagmentNET.Models;
using StaffManagmentNET.Repositories;
using StaffManagmentNET.Responses;
using StaffManagmentNET.ViewModels;
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

        public async Task CheckIn(CheckInVM vm)
        {
            var device = await _context.Devices
                .Where(d => d.StaffID == vm.StaffID && d.DeviceType.Contains("Laptop") || d.DeviceType.Contains("PC"))
                .FirstOrDefaultAsync();

            if (device != null && device.PublicIP != vm.PublicIP)
            {
                throw new Exception("Please check-in by own company device!");
            }

            var timeSheet = await _context.TimeSheets.FindAsync(DateTime.Now.ToString("dd/MM/yyyy"), vm.StaffID);

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

            var newTimeSheet = new TimeSheet
            {
                Date = DateTime.Now.ToString("dd/MM/yyyy"),
                StaffID = vm.StaffID,
                CheckIn = DateTime.Now,
                WorkingType = "OFFICE",
                Off = "NO"
            };
            _context.TimeSheets.Add(newTimeSheet);

            await _context.SaveChangesAsync();

            var coreTime = await _context.Settings.FindAsync("Thời gian vào làm");
            if (coreTime == null)
            {
                return;
            }

            var start = coreTime.Value.Split(':');
            if (start.Length < 1) return;
            var h = int.Parse(start[0]);
            var m = int.Parse(start[1]);
            if (h > newTimeSheet.CheckIn.Hour || (h == newTimeSheet.CheckIn.Hour && m > newTimeSheet.CheckIn.Minute))
            {
                throw new Exception("Checked-in after start time! You got a default.");
            }
        }

        public async Task CheckOut(CheckInVM vm)
        {
            var timeSheet = await _context.TimeSheets.FindAsync(DateTime.Now.ToString("dd/MM/yyyy"), vm.StaffID);

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

            var coreTime = await _context.Settings.FindAsync("Thời gian kết thúc");
            if (coreTime == null)
            {
                return;
            }

            var end = coreTime.Value.Split(':');
            if (end.Length < 1) return;
            var h = int.Parse(end[0]);
            var m = int.Parse(end[1]);
            if (h < timeSheet.CheckOut.Hour || (h == timeSheet.CheckOut.Hour && m < timeSheet.CheckOut.Minute))
            {
                throw new Exception("Checked-out before end time! You got a default.");
            }
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

            return Math.Round(totalThisMonth, 2);
        }

        public async Task<TimeSheet> GetTimeSheetByDay(string staffID, int day, int month, int year)
        {
            var date = day < 10 ? $"0{day}" : day.ToString();
            date += month < 10 ? $"/0{month}/" : $"/{month}/";
            date += year.ToString();
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
            var total = (timeSheet.CheckOut - timeSheet.CheckIn).TotalHours - timeSheet.LunchBreakHour;

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

        public async Task GenerateData(string staffID)
        {
            for (int i = 2; i <= 31; i++)
            {
                if ((i - 4) % 7 == 0 || (i - 5) % 7 == 0) continue;

                var date = i < 10 ? "0" + i.ToString() : i.ToString();
                date += "/05/2024";
                var ts = await _context.TaskDetails.FindAsync(date, staffID, 1);
                if (ts == null)
                {
                    ts = new TaskDetail
                    {
                        Date = date,
                        StaffID = staffID,
                        StartTime = new DateTime(2024, 5, i, 8, 55, 0),
                        EndTime = new DateTime(2024, 5, i, 11, 01, 0),
                        Status = "Done",
                        Estimate = 4,
                        Evaluate = 77,
                        Feedback = "gudd"
                    };
                    _context.TaskDetails.Add(ts);
                    continue;
                }

                ts.StartTime = new DateTime(2024, 5, i, 8, 55, 0);
                ts.EndTime = new DateTime(2024, 5, i, 11, 01, 0);
                ts.Evaluate = 77;
                ts.Status = "Done";
                ts.Feedback = "gud";
            }

            await _context.SaveChangesAsync();
        }
    }
}
