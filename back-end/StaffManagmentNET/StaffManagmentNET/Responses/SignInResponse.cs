using StaffManagmentNET.Models;

namespace StaffManagmentNET.Responses
{
    public class SignInResponse
    {
        public string? AccessToken { get; set; }
        public string? Roles { get; set; }
        public string? StaffID { get; set; }
        public string? StaffName { get; set; }
    }
}
