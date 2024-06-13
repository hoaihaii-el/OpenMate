using StaffManagmentNET.Models;
using StaffManagmentNET.Responses;
using StaffManagmentNET.ViewModels;

namespace StaffManagmentNET.Repositories
{
    public interface ITimeSheetRepo
    {
        Task CheckIn(CheckInVM vm);
        Task CheckOut(CheckInVM vm);
        Task<TimeSheet> GetTimeSheetByDay(string staffID, int day, int month, int year);
        Task<IEnumerable<TimeSheetResponse>> GetTimeSheetByMonth(string staffID, int month, int year);
        Task<double> GetAvgByMonth(string staffID, int month, int year);
        Task<double> GetTotalByMonth(string staffID, int month, int year);
        Task<TimeSheetDetail> GetSheetDetail(string staffID, int month, int year);
        Task<TimeSheet> UpdateData(string staffID, string date, int h1, int m1, int h2, int m2, string type, string wrkType, string off);
    }
}
