using StaffManagmentNET.Models;

namespace StaffManagmentNET.Repositories
{
    public interface IDivisionRepo
    {
        Task<Division> CreateDivision(string divisionName, string managerID);
        Task<Division> UpdateManager(string divisionID, string managerID);
        Task<Division> DeleteDivision(string divisionID);
    }
}
