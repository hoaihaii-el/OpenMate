using StaffManagmentNET.Models;

namespace StaffManagmentNET.Repositories
{
    public interface ISettingRepo
    {
        Task<IEnumerable<Setting>> GetAll();
        Task UpdateAll(List<Setting> settings);
    }
}
