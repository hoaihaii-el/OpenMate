using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using StaffManagmentNET.Hubs;
using StaffManagmentNET.Repositories;
using StaffManagmentNET.ViewModels;

namespace StaffManagmentNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskDetailController : ControllerBase
    {
        private readonly ITaskRepo _service;
        private readonly IHubContext<TaskHub> _hub;

        public TaskDetailController(ITaskRepo service, IHubContext<TaskHub> hub)
        {
            _service = service;
            _hub = hub;
        }

        [HttpGet("user-get")]
        public async Task<IActionResult> GetByDay(string date, string staffID)
        {
            return Ok(await _service.GetUserTask(date, staffID));
        }

        [HttpPost("new-task")]
        public async Task<IActionResult> NewTask(TaskVM task)
        {
            await _service.UpdateTask(task);
            await _hub.Clients.All.SendAsync("UpdateTask", "");
            return Ok();
        }

        [HttpGet("manager-get")]
        public async Task<IActionResult> UpdateTask(string date, string managerID)
        {
            return Ok(await _service.GetUserTaskForManager(date, managerID));
        }
    }
}
