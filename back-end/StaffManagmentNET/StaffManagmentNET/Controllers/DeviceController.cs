using Microsoft.AspNetCore.Mvc;
using StaffManagmentNET.Models;

namespace StaffManagmentNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllDevice()
        {
            return Ok();
        }

        [HttpGet("detail/{deviceID}")]
        public IActionResult GetDevice(string deviceID)
        {
            return Ok();
        }

        [HttpPost("new-device")]
        public IActionResult AddNewComer(Device device)
        {
            return Ok();
        }

        [HttpPut("update/{deviceID}")]
        public IActionResult UpdateInfo(string deviceID, Device device)
        {
            return Ok();
        }
    }
}
