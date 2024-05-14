using StaffManagmentNET.Models;

namespace StaffManagmentNET.Responses
{
    public class SignInResponse
    {
        public string? AccessToken { get; set; }
        public string? ID { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public Staff? Staff { get; set; }
    }
}
