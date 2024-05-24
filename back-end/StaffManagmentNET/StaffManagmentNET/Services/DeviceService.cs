using Microsoft.EntityFrameworkCore;
using StaffManagmentNET.Models;
using StaffManagmentNET.Repositories;
using StaffManagmentNET.Responses;
using StaffManagmentNET.ViewModels;

namespace StaffManagmentNET.Services
{
    public class DeviceService : IDeviceRepo
    {
        private readonly DataContext _context;

        public DeviceService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DeviceResponse>> GetAll()
        {
            var devices = await _context.Devices.ToListAsync();

            var result = new List<DeviceResponse>();

            foreach (var device in devices)
            {
                var res = new DeviceResponse
                {
                    DeviceID = device.DeviceID,
                    DeviceName = device.DeviceName,
                    DeviceType = device.DeviceType,
                    Condition = device.Condition,
                    StaffID = device.StaffID,
                };

                if (!string.IsNullOrEmpty(device.StaffID))
                {
                    var staff = await _context.Staffs.FindAsync(device.StaffID);
                    res.StaffName = staff!.StaffName;
                }
                else
                {
                    res.StaffName = "No owner yet";
                }

                result.Add(res);
            }

            return result;
        }

        public async Task NewDevice(DeviceVM vm)
        {
            var deviceCheck = await _context.Devices.FindAsync(vm.DeviceID);
            if (deviceCheck != null)
            {
                throw new Exception("Device existed!");
            }

            if (vm.StaffID != "Empty")
            {
                var staff = await _context.Staffs.FindAsync(vm.StaffID);
                if (staff == null)
                {
                    throw new Exception("Owner not found!");
                }
            }

            _context.Devices.Add(new Device
            {
                DeviceID = vm.DeviceID,
                DeviceName = vm.DeviceName,
                DeviceType = vm.DeviceType,
                StaffID = vm.StaffID == "Empty" ? "" : vm.StaffID,
                Condition = vm.Condition,
            });

            await _context.SaveChangesAsync();
        }

        public async Task UpdateDevice(DeviceVM vm)
        {
            var device = await _context.Devices.FindAsync(vm.DeviceID);

            if (device == null)
            {
                throw new ArgumentNullException("Device not found!");
            }

            if (vm.StaffID != "Empty")
            {
                var staff = await _context.Staffs.FindAsync(vm.StaffID);
                if (staff == null)
                {
                    throw new Exception("Owner not found!");
                }
            }

            device.DeviceName = vm.DeviceName;
            device.DeviceType = vm.DeviceType;
            device.StaffID = vm.StaffID == "Empty" ? "" : vm.StaffID;
            device.Condition = vm.Condition;

            _context.Devices.Update(device);

            await _context.SaveChangesAsync();
        }
    }
}
