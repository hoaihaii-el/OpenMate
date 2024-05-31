using Microsoft.AspNetCore.Mvc;
using StaffManagmentNET.Repositories;
using StaffManagmentNET.ViewModels;

namespace StaffManagmentNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskDetailController : ControllerBase
    {
        private readonly ITaskRepo _service;

        public TaskDetailController(ITaskRepo service)
        {
            _service = service;
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
            return Ok();
        }

        [HttpGet("manager-get")]
        public async Task<IActionResult> UpdateTask(string date, string managerID)
        {
            return Ok(await _service.GetUserTaskForManager(date, managerID));
        }
    }
}
