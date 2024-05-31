using Microsoft.EntityFrameworkCore;
using StaffManagmentNET.Models;
using StaffManagmentNET.Repositories;
using StaffManagmentNET.Responses;
using StaffManagmentNET.ViewModels;

namespace StaffManagmentNET.Services
{
    public class TaskService : ITaskRepo
    {
        private readonly DataContext _context;

        public TaskService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskResponse>> GetUserTask(string date, string staffID)
        {
            var tasks = await _context.TaskDetails
                .Where(t => t.Date == date && t.StaffID == staffID)
                .OrderBy(t => t.Order)
                .ToListAsync();

            var res = new List<TaskResponse>();
            foreach (var task in tasks)
            {
                var t = new TaskResponse
                {
                    Date = task.Date,
                    StaffID = task.StaffID,
                    StaffName = "",
                    Order = task.Order,
                    TaskName = task.TaskName,
                    Status = task.Status,
                    Estimate = task.Estimate.ToString(),
                    Note = task.Note,
                    Evaluate = task.Evaluate.ToString(),
                    Feedback = task.Feedback,
                    StartTime = $"{task.StartTime.Hour}h{task.StartTime.Minute}",
                };
                if (task.EndTime > DateTime.MinValue)
                {
                    t.EndTime = $"{task.EndTime.Hour}h{task.EndTime.Minute}";
                }
                else
                {
                    t.EndTime = "__h__";
                }

                res.Add(t);
            }

            return res;
        }

        public async Task<IEnumerable<TaskResponse>> GetUserTaskForManager(string date, string staffID)
        {
            var staffs = await _context.Staffs.Where(s => s.ManagerID == staffID).ToListAsync();

            var result = new List<TaskResponse>();
            var tasks = await GetUserTask(date, staffID) as List<TaskResponse>;
            if (tasks!.Count > 0) 
            {
                var staff = await _context.Staffs.FindAsync(staffID);
                tasks[0].StaffName = staff!.StaffName;
                result.AddRange(tasks);
            }

            foreach (var staff in staffs)
            {
                var list = await GetUserTask(date, staff.StaffID) as List<TaskResponse>;
                if (list!.Count == 0) continue;

                list[0].StaffName  = staff.StaffName;
                result.AddRange(list);
            }

            return result;
        }

        public async Task UpdateTask(TaskVM vm)
        {
            var task = await _context.TaskDetails.FindAsync(vm.Date, vm.StaffID, vm.Order);
            vm.Date = vm.Date.Replace("%2F", "/");
            if (task == null)
            {
                var newTask = new TaskDetail
                {
                    Date = vm.Date,
                    StaffID = vm.StaffID,
                    Order = vm.Order,
                    TaskName = vm.TaskName,
                    Status = vm.Status,
                    Estimate = vm.Estimate,
                    Note = vm.Note,
                    Evaluate = vm.Evaluate,
                    Feedback = vm.Feedback,
                };
                var now = DateTime.Now;
                var startTime = vm.StartTime.Split('h');
                if (startTime.Count() > 1)
                {
                    newTask.StartTime = new DateTime(now.Year, now.Month, now.Day, int.Parse(startTime[0]), int.Parse(startTime[1]), 0);
                }
                var endTime = vm.EndTime.Split('h');
                if (endTime.Count() > 1)
                {
                    newTask.EndTime = new DateTime(now.Year, now.Month, now.Day, int.Parse(endTime[0]), int.Parse(endTime[1]), 0);
                }
                _context.TaskDetails.Add(newTask);
                await _context.SaveChangesAsync();
                return;
            }

            task.TaskName = vm.TaskName;
            task.Status = vm.Status;
            task.Estimate = vm.Estimate;
            task.Note = vm.Note;
            task.Evaluate = vm.Evaluate;
            task.Feedback = vm.Feedback;

            var start = vm.StartTime.Split('h');
            if (start.Count() > 1)
            {
                task.StartTime = new DateTime(task.StartTime.Year, task.StartTime.Month, task.StartTime.Day, int.Parse(start[0]), int.Parse(start[1]), 0);
            }

            var end = vm.EndTime.Split('h');
            if (end.Count() > 1)
            {
                task.EndTime = new DateTime(task.EndTime.Year, task.EndTime.Month, task.EndTime.Day, int.Parse(end[0]), int.Parse(end[1]), 0);
            }
            _context.TaskDetails.Update(task);
            await _context.SaveChangesAsync();
        }
    }
}
