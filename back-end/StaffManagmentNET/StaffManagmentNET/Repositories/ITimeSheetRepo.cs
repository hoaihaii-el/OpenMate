namespace StaffManagmentNET.Repositories
{
    public interface ITimeSheetRepo
    {
        Task CheckIn(string staffID);
        Task CheckOut(string staffID);
        Task ReCheckIn(string staffID, DateTime datetime);
        Task ReCheckOut(string staffID, DateTime datetime);
        Task<double> GetAvgByMonth(string staffID, int month, int year);
        Task<double> GetTotalByMonth(string staffID, int month, int year);
    }
}
