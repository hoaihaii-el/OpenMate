using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StaffManagmentNET.Models;

namespace StaffManagmentNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllNoti()
        {
            return Ok();
        }

        [HttpGet("detail/{notiID}")]
        public IActionResult GetStringStaff(string notiID)
        {
            return Ok();
        }

        [HttpPost("new-noti")]
        public IActionResult AddNewComer(Notification noti)
        {
            return Ok();
        }
    }
}
