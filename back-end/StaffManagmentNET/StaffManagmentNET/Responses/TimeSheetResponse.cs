namespace StaffManagmentNET.Responses
{
    public class TimeSheetResponse
    {
        public string Date { get; set; } = "";
        public string CheckIn { get; set; } = "";
        public string CheckOut { get; set; } = "";
        public string LunchBreak { get; set; } = "";
        public string Total { get; set; } = "";
        public string WorkingType { get; set; } = "";
        public string Off { get; set; } = "";
        public string Key { get; set; } = "";
        public int Today { get; set; } = 0;
        public bool IsDayOff { get; set; }
    }
}
