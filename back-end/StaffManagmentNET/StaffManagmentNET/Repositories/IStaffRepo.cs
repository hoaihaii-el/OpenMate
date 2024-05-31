using StaffManagmentNET.Models;
using StaffManagmentNET.Responses;
using StaffManagmentNET.ViewModels;

namespace StaffManagmentNET.Repositories
{
    public interface IStaffRepo
    {
        Task<IEnumerable<StaffResponse>> GetAll();
        Task<StaffResponse> UpdatePersonalInfo(UserUpdateInfoVM vm);
        Task HRUpdateInfo(HRUpdateVM vm);
        Task<StaffResponse> GetUserInfo(string staffID);
    }
}
