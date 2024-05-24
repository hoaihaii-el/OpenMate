namespace StaffManagmentNET.Responses
{
    public class DeviceResponse
    {
        public string DeviceID { get; set; } = "";
        public string DeviceName { get; set; } = "";
        public string DeviceType { get; set; } = "";
        public string? StaffID { get; set; }
        public string StaffName { get; set; } = "";
        public string Condition { get; set; } = "";
    }
}
