namespace StaffManagmentNET.ViewModels
{
    public class TaskVM
    {
        public string Date { get; set; } = "";
        public string StaffID { get; set; } = "";
        public int Order { get; set; }
        public string TaskName { get; set; } = "";
        public string Status { get; set; } = "";
        public double Estimate { get; set; } = 1;
        public string Note { get; set; } = "";
        public double Evaluate { get; set; } = 50;
        public string Feedback { get; set; } = "";
        public string StartTime { get; set; } = "";
        public string EndTime { get; set; } = "";
    }
}
