using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StaffManagmentNET.Models;
using StaffManagmentNET.Repositories;
using StaffManagmentNET.ViewModels;

namespace StaffManagmentNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotification _service;
        public NotificationController(INotification service) 
        {
            _service = service;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllNoti()
        {
            return Ok(await _service.GetAllNoti());
        }

        [HttpPost("new-noti")]
        public async Task<IActionResult> NewNoti(NotificationVM vm)
        {
            await _service.NewNotification(vm);
            return Ok();
        }
    }
}
