using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StaffManagmentNET.Models;

namespace StaffManagmentNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskDetailController : ControllerBase
    {
        [HttpGet("get-by-day/{date}")]
        public IActionResult GetByDay(string date)
        {
            return Ok();
        }

        [HttpPost("new-task")]
        public IActionResult NewTask(TaskDetail task)
        {
            return Ok();
        }

        [HttpPost("update-task")]
        public IActionResult UpdateTask(TaskDetail task)
        {
            return Ok();
        }
    }
}
