using Microsoft.AspNetCore.Identity;
using StaffManagmentNET.Models;
using StaffManagmentNET.Responses;
using StaffManagmentNET.ViewModels;

namespace StaffManagmentNET.Repositories
{
    public interface IAccountRepo
    {
        public Task<SignInResponse> SignIn(SigninVM model);
        public Task<Staff> Register(RegisterVM model);
        public Task ChangePassword(string id, string oldPw, string newPw);
    }
}
