using Microsoft.AspNetCore.Mvc;
using StaffManagmentNET.Models;

namespace StaffManagmentNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivisionController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllDivision()
        {
            return Ok();
        }

        [HttpPost("new-division")]
        public IActionResult AddNewComer(Division division)
        {
            return Ok();
        }

        [HttpPut("update/{divisionID}")]
        public IActionResult UpdateInfo(string divisionID, Division division)
        {
            return Ok();
        }
    }
}
