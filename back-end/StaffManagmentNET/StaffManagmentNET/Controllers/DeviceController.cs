using Microsoft.AspNetCore.Mvc;
using StaffManagmentNET.Models;
using StaffManagmentNET.Repositories;
using StaffManagmentNET.ViewModels;

namespace StaffManagmentNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceRepo _service;

        public DeviceController(IDeviceRepo service)
        {
            _service = service;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllDevice()
        {
            return Ok(await _service.GetAll());
        }

        [HttpPost("new-device")]
        public async Task<IActionResult> NewDevice(DeviceVM vm)
        {
            try
            {
                await _service.NewDevice(vm);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateInfo(DeviceVM device)
        {
            try
            {
                await _service.UpdateDevice(device);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
