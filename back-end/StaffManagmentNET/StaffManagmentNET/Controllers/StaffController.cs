using Microsoft.AspNetCore.Mvc;
using StaffManagmentNET.Repositories;
using StaffManagmentNET.ViewModels;

namespace StaffManagmentNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffRepo _service;

        public StaffController(IStaffRepo service)
        {
            _service = service;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllStaff()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet("detail")]
        public async Task<IActionResult> GetDetail(string staffID)
        {
            try
            {
                return Ok(await _service.GetUserInfo(staffID));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("hr-update")]
        public async Task<IActionResult> HRUpdate(HRUpdateVM vm)
        {
            await _service.HRUpdateInfo(vm);
            return Ok();
        }

        [HttpPut("user-update")]
        public async Task<IActionResult> UserUpdate(UserUpdateInfoVM vm)
        {
            await _service.UpdatePersonalInfo(vm);
            return Ok();
        }
    }
}
