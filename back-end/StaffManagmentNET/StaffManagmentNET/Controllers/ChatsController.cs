using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using StaffManagmentNET.Hubs;
using StaffManagmentNET.Repositories;
using StaffManagmentNET.ViewModels;

namespace StaffManagmentNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IChatRepo _service;
        private readonly IHubContext<ChatHub> _hub;

        public ChatsController(IChatRepo service, IHubContext<ChatHub> hub)
        {
            _service = service;
            _hub = hub;
        }

        [HttpGet("get-all-rooms")]
        public async Task<IActionResult> GetAllRoom(string staffID)
        {
            return Ok(await _service.GetAllRoom(staffID));
        }

        [HttpGet("get-messages")]
        public async Task<IActionResult> GetMessages(string roomID)
        {
            return Ok(await _service.GetAllMessage(roomID));
        }

        [HttpPost("add-new-room")]
        public async Task<IActionResult> AddNewRoom(string users)
        {
            try
            {
                await _service.AddNewRoom(users);
                await _hub.Clients.All.SendAsync("ReceiveMessage", "");
                return Ok();
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-room-detail")]
        public async Task<IActionResult> GetRoomDetail(string roomID)
        {
            return Ok(await _service.GetRoomDetail(roomID));
        }

        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage(MessageVM vm)
        {
            await _service.SendMessage(vm);
            await _hub.Clients.All.SendAsync("ReceiveMessage", vm.Content);
            return Ok();
        }
    }
}
