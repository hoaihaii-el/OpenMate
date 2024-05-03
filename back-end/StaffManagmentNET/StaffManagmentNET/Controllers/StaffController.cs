using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StaffManagmentNET.Models;

namespace StaffManagmentNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllStaff()
        {
            return Ok();
        }

        [HttpGet("detail/{staffID}")]
        public IActionResult GetStaff(string staffID)
        {
            return Ok();
        }

        [HttpPost("new-comer")]
        public IActionResult AddNewComer(Staff staff)
        {
            return Ok();
        }

        [HttpPut("update-info/{staffID}")]
        public IActionResult UpdateInfo(string staffID, Staff staff)
        {
            return Ok();
        }
    }
}
