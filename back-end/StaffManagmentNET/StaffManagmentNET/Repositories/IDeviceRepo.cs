using StaffManagmentNET.Responses;
using StaffManagmentNET.ViewModels;

namespace StaffManagmentNET.Repositories
{
    public interface IDeviceRepo
    {
        Task<IEnumerable<DeviceResponse>> GetAll();
        Task NewDevice(DeviceVM vm);
        Task UpdateDevice(DeviceVM vm);
    }
}
