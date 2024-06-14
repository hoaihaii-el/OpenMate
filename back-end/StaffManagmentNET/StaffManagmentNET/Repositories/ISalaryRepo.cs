using StaffManagmentNET.Models;
using StaffManagmentNET.ViewModels;

namespace StaffManagmentNET.Repositories
{
    public interface ISalaryRepo
    {
        Task<IEnumerable<Salary>> GetSalary(int month, int year);
        Task AddReward(List<RewardVM> vms);
    }
}
