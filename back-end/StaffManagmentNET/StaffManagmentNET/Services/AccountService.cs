using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StaffManagmentNET.Models;
using StaffManagmentNET.Repositories;
using StaffManagmentNET.Responses;
using StaffManagmentNET.StaticServices;
using StaffManagmentNET.ViewModels;
using System.Text.RegularExpressions;

namespace StaffManagmentNET.Services
{
    public class AccountService : IAccountRepo
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWTManager _JWTManager;
        private readonly DataContext _context;

        public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            JWTManager JWTManager, DataContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
            _JWTManager = JWTManager;
        }

        public async Task<Staff> Register(RegisterVM register)
        {
            var user = new AppUser
            {
                Id = register.StaffID,
                UserName = Guid.NewGuid().ToString(),
                Email = Guid.NewGuid().ToString() + "@gmail.com"
            };

            if (!await _roleManager.RoleExistsAsync(AppRole.Admin))
            {
                await _roleManager.CreateAsync(
                    new IdentityRole(AppRole.Admin)
                );
            }

            if (!await _roleManager.RoleExistsAsync(AppRole.Staff))
            {
                await _roleManager.CreateAsync(
                    new IdentityRole(AppRole.Staff)
                );
            }

            if (!await _roleManager.RoleExistsAsync(AppRole.CEO))
            {
                await _roleManager.CreateAsync(
                    new IdentityRole(AppRole.CEO)
                );
            }

            if (!await _roleManager.RoleExistsAsync(AppRole.Accountant))
            {
                await _roleManager.CreateAsync(
                    new IdentityRole(AppRole.Accountant)
                );
            }

            if (!await _roleManager.RoleExistsAsync(AppRole.HRManager))
            {
                await _roleManager.CreateAsync(
                    new IdentityRole(AppRole.HRManager)
                );
            }

            if (!await _roleManager.RoleExistsAsync(AppRole.HRStaff))
            {
                await _roleManager.CreateAsync(
                    new IdentityRole(AppRole.HRStaff)
                );
            }

            if (!await _roleManager.RoleExistsAsync(AppRole.DivisionManager))
            {
                await _roleManager.CreateAsync(
                    new IdentityRole(AppRole.DivisionManager)
                );
            }

            var staff = new Staff
            {
                StaffID = user.Id,
                StaffName = register.StaffName,
                Title = register.Title,
                Level = register.Level,
                Male = register.Male,
                Address = register.Address,
                DateBirth = DateTime.Parse(register.DateBirth),
                StartWork = DateTime.Parse(register.StartWork),
                ManagerID = register.ManagerID == "Empty" ? "" : register.ManagerID,
                DivisionID = register.DivisionID,
                AvatarURL = "https://static.vecteezy.com/system/resources/previews/009/292/244/original/default-avatar-icon-of-social-media-user-vector.jpg"
            };

            staff.Division = await _context.Divisions.FindAsync(staff.DivisionID);

            _context.Staffs.Add(staff);
            await _userManager.CreateAsync(user, register.Password);

            foreach (var role in register.Roles.Split("_", StringSplitOptions.TrimEntries))
            {
                await _userManager.AddToRoleAsync(user, role);
            }
            await _context.SaveChangesAsync();

            return staff;
        }

        public async Task<SignInResponse> SignIn(SigninVM vm)
        {
            var user = await _userManager.FindByIdAsync(vm.UserID);

            if (user == null)
            {
                throw new ArgumentNullException("User not found!");
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, vm.Password);

            if (!passwordValid)
            {
                throw new KeyNotFoundException("Incorrect password!");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            return new SignInResponse
            {
                AccessToken = _JWTManager.GenerateToken(user.Id, (List<string>)userRoles),
                Roles = string.Join('_', userRoles),
                StaffID = user.Id
            };
        }

        public async Task ChangePassword(string id, string oldPw, string newPw)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var pwHasher = new PasswordHasher<AppUser>();
            if (pwHasher.VerifyHashedPassword(user, user.PasswordHash!, oldPw) == PasswordVerificationResult.Failed)
            {
                throw new Exception("Password verified failed");
            }

            user.PasswordHash = pwHasher.HashPassword(user, newPw);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception("Changed password failed!");
            }
        }
    }
}
