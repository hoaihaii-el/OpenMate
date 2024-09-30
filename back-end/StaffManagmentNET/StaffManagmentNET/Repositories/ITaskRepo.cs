using StaffManagmentNET.Responses;
using StaffManagmentNET.ViewModels;

namespace StaffManagmentNET.Repositories
{
    public interface ITaskRepo
    {
        Task<IEnumerable<TaskResponse>> GetUserTask(string date, string staffID);
        Task<IEnumerable<TaskResponse>> GetUserTaskForManager(string date, string staffID);
        Task UpdateTask(TaskVM vm);
    }
}
