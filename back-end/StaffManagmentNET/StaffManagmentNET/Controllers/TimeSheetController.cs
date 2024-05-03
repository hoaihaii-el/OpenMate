using Microsoft.AspNetCore.Mvc;
using StaffManagmentNET.Models;

namespace StaffManagmentNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSheetController : ControllerBase
    {
        [HttpGet("get-by-month/{month}/{year}")]
        public IActionResult GetByMonth(int month, int year)
        {
            return Ok();
        }

        [HttpGet("get-by-day/{day}/{month}/{year}")]
        public IActionResult GetStringStaff(int day, int month, int year)
        {
            return Ok();
        }

        [HttpPost("add")]
        public IActionResult AddNewComer(TimeSheet sheet)
        {
            return Ok();
        }

        [HttpPost("update/{date}/{staffID}")]
        public IActionResult UpdateTimeSheet(DateTime date, string staffID, TimeSheet sheet)
        {
            return Ok();
        }
    }
}
