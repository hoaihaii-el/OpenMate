using Microsoft.AspNetCore.Mvc;
using StaffManagmentNET.Models;
using StaffManagmentNET.Repositories;

namespace StaffManagmentNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivisionController : ControllerBase
    {
        private readonly IDivisionRepo _service;

        public DivisionController(IDivisionRepo service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAllDivision()
        {
            return Ok();
        }

        [HttpPost("new-division")]
        public async Task<IActionResult> AddNewComer(string name, string managerID)
        {
            return Ok(new
            {
                success = true,
                data = await _service.CreateDivision(name, managerID)
            });
        }

        [HttpPut("update-manager")]
        public async Task<IActionResult> UpdateManager(string divisionID, string managerID)
        {
            try
            {
                return Ok(new
                {
                    success = true,
                    data = await _service.UpdateManager(divisionID, managerID)
                });
            }
            catch(Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete("delete/{divisionID}")]
        public async Task<IActionResult> DeleteDivision(string divisionID)
        {
            try
            {
                return Ok(new
                {
                    success = true,
                    data = await _service.DeleteDivision(divisionID)
                });
            }
            catch(Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                });
            }
        }
    }
}
