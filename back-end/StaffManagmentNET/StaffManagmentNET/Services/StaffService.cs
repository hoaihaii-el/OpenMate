using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StaffManagmentNET.Models;
using StaffManagmentNET.Repositories;
using StaffManagmentNET.Responses;
using StaffManagmentNET.ViewModels;
using System.Globalization;

namespace StaffManagmentNET.Services
{
    public class StaffService : IStaffRepo
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;

        public StaffService(DataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<StaffResponse>> GetAll()
        {
            var staffs = await _context.Staffs.ToListAsync();
            var result = new List<StaffResponse>();
            foreach (var staff in staffs)
            {
                var res = new StaffResponse
                {
                    StaffID = staff.StaffID,
                    StaffName = staff.StaffName,
                    Gender = staff.Male ? "Nam" : "Nữ",
                    DateBirth = staff.DateBirth.ToString("dd/MM/yyyy"),
                    Phone = staff.Phone,
                    Address = staff.Address, 
                    StartWork = staff.StartWork.ToString("dd/MM/yyyy"),
                    StartFullTime = staff.StartFullTime > DateTime.MinValue ? staff.StartFullTime.ToString("dd/MM/yyyy") : "",
                    Title = staff.Title,
                    Level = staff.Level,
                    CompanyEmail = staff.CompanyEmail,
                    PersonalEmail = staff.PersonalEmail,
                    AvatarURL = staff.AvatarURL,
                    DivisionID = staff.DivisionID,
                    ManagerID = staff.ManagerID,
                    BHXH = staff.BHXH,
                    BHTN = staff.BHTN,
                    BHYT = staff.BHYT,
                    MaSoThue = staff.MaSoThue,
                    HDLD = staff.HDLD,
                    SoHDLD = staff.SoHDLD,
                };

                if (!string.IsNullOrEmpty(staff.ManagerID))
                {
                    var manager = await _context.Staffs.FindAsync(staff.ManagerID);
                    if (manager == null)
                    {
                        throw new Exception("Manager not found!");
                    }
                    res.ManagerName = manager.StaffName;
                }
                var user = await _userManager.FindByIdAsync(staff.StaffID);
                var roles = await _userManager.GetRolesAsync(user!);
                res.Roles = string.Join("_", roles);

                var divisions = await _context.Divisions.Select(d => d.DivisionID).ToListAsync();
                res.AllDivision = string.Join("_", divisions);

                result.Add(res);
            }

            return result;
        }

        public async Task<StaffResponse> GetUserInfo(string staffID)
        {
            var staff = await _context.Staffs.FindAsync(staffID);
            if (staff == null)
            {
                throw new ArgumentNullException("Staff not found!");
            }

            var res = new StaffResponse
            {
                StaffID = staff.StaffID,
                StaffName = staff.StaffName,
                Gender = staff.Male ? "Nam" : "Nữ",
                DateBirth = staff.DateBirth.ToString("dd/MM/yyyy"),
                Phone = staff.Phone,
                Address = staff.Address,
                StartWork = staff.StartWork.ToString("dd/MM/yyyy"),
                StartFullTime = staff.StartFullTime.ToString("dd/MM/yyyy"),
                Title = staff.Title,
                Level = staff.Level,
                CompanyEmail = staff.CompanyEmail,
                PersonalEmail = staff.PersonalEmail,
                AvatarURL = staff.AvatarURL,
                DivisionID = staff.DivisionID,
                ManagerID = staff.ManagerID,
                BHXH = staff.BHXH,
                BHTN = staff.BHTN,
                BHYT = staff.BHYT,
                MaSoThue = staff.MaSoThue,
                HDLD = staff.HDLD,
                SoHDLD = staff.SoHDLD,
                BankAccount = staff.BankAccount,
                BankName = staff.BankName
            };

            var user = await _userManager.FindByIdAsync(staff.StaffID);
            var roles = await _userManager.GetRolesAsync(user!);
            res.Roles = string.Join("_", roles);

            return res;
        }

        public async Task HRUpdateInfo(HRUpdateVM vm)
        {
            var staff = await _context.Staffs.FindAsync(vm.StaffID);
            if (staff == null)
            {
                throw new ArgumentNullException("Staff not found!");
            }

            staff.StaffName = vm.StaffName;
            staff.DivisionID = vm.DivisionID;
            staff.StartFullTime = vm.StartFullTime != "Empty" ? DateTime.Parse(vm.StartFullTime) : staff.StartFullTime;
            staff.Title = vm.Title;
            staff.Level = vm.Level;
            staff.ManagerID = vm.ManagerID == "Empty" ? "" : vm.ManagerID;
            staff.BHXH = vm.BHXH == "Empty" ? "" : vm.BHXH;
            staff.BHYT = vm.BHYT == "Empty" ? "" : vm.BHYT;
            staff.BHTN = vm.BHTN == "Empty" ? "" : vm.BHTN;
            staff.MaSoThue = vm.MaSoThue == "Empty" ? "" : vm.MaSoThue;
            staff.HDLD = vm.HDLD == "Empty" ? "" : vm.HDLD;
            staff.SoHDLD = vm.SoHDLD == "Empty" ? "" : vm.SoHDLD;

            var user = await _userManager.FindByIdAsync(vm.StaffID);
            foreach (var role in vm.Roles.Split('_', StringSplitOptions.TrimEntries))
            {
                try
                {
                    await _userManager.AddToRoleAsync(user!, role);
                }
                catch
                {
                    continue;
                }
            }

            _context.Staffs.Update(staff);
            await _context.SaveChangesAsync();
        }

        public async Task<StaffResponse> UpdatePersonalInfo(UserUpdateInfoVM vm)
        {
            var staff = await _context.Staffs.FindAsync(vm.StaffID);

            if (staff == null)
            {
                throw new Exception("Staff not found!");
            }

            staff.StaffName = vm.StaffName;
            staff.DateBirth = DateTime.ParseExact(vm.DateBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            staff.Male = vm.Gender == "Nam";
            staff.Address = vm.Address;
            staff.Phone = vm.Phone;
            staff.PersonalEmail = vm.PersonalEmail;
            staff.CompanyEmail = vm.CompanyEmail;
            staff.BankAccount = vm.BankAccount;
            staff.BankName = vm.BankName;

            _context.Staffs.Update(staff);
            await _context.SaveChangesAsync();

            return new StaffResponse
            {
                StaffID = staff.StaffID,
                StaffName = staff.StaffName,
                Gender = staff.Male ? "Nam" : "Nữ",
                DateBirth = staff.DateBirth.ToString("dd/MM/yyyy"),
                Phone = staff.Phone,
                Address = staff.Address,
                StartWork = staff.StartWork.ToString("dd/MM/yyyy"),
                StartFullTime = staff.StartFullTime.ToString("dd/MM/yyyy"),
                Title = staff.Title,
                Level = staff.Level,
                CompanyEmail = staff.CompanyEmail,
                PersonalEmail = staff.PersonalEmail,
                AvatarURL = staff.AvatarURL,
                DivisionID = staff.DivisionID,
                ManagerID = staff.ManagerID,
                BHXH = staff.BHXH,
                BHTN = staff.BHTN,
                BHYT = staff.BHYT,
                MaSoThue = staff.MaSoThue,
                HDLD = staff.HDLD,
                SoHDLD = staff.SoHDLD,
                BankAccount = staff.BankAccount,
                BankName = staff.BankName
            };
        }
    }
}
