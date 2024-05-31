namespace StaffManagmentNET.Responses
{
    public class TaskResponse
    {
        public string Date { get; set; } = "";
        public string StaffID { get; set; } = "";
        public string StaffName { get; set; } = "";
        public int Order { get; set; }
        public string TaskName { get; set; } = "";
        public string Status { get; set; } = "";
        public string Estimate { get; set; } = "";
        public string Note { get; set; } = "";
        public string Evaluate { get; set; } = "";
        public string Feedback { get; set; } = "";
        public string StartTime { get; set; } = "";
        public string EndTime { get; set; } = "";
    }
}
