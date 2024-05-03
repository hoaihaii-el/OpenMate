using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StaffManagmentNET.Models;

namespace StaffManagmentNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyInfoController : ControllerBase
    {
        [HttpGet("staffID")]
        public IActionResult GetAll(string staffID)
        {
            return Ok();
        }

        [HttpPost("new-info")]
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
