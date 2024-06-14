using Microsoft.AspNetCore.Mvc;
using StaffManagmentNET.Repositories;
using StaffManagmentNET.ViewModels;

namespace StaffManagmentNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalariesController : ControllerBase
    {
        private readonly ISalaryRepo _service;

        public SalariesController(ISalaryRepo service)
        {
            _service = service;
        }


        [HttpGet("get-salary")]
        public async Task<IActionResult> GetSalary(int month, int year)
        {
            return Ok(await _service.GetSalary(month, year));
        }

        [HttpPut("add-reward")]
        public async Task<IActionResult> AddReward(List<RewardVM> vms)
        {
            await _service.AddReward(vms);
            return Ok();
        }
    }
}
