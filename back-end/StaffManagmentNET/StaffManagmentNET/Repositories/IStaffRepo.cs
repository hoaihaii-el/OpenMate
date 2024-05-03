using StaffManagmentNET.Models;

namespace StaffManagmentNET.Repositories
{
    public interface IStaffRepo
    {
        Task<Staff> AddNewComer();
    }
}
