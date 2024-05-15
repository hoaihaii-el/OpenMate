using Microsoft.AspNetCore.Mvc;
using StaffManagmentNET.Models;
using StaffManagmentNET.Repositories;

namespace StaffManagmentNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSheetController : ControllerBase
    {
        private readonly ITimeSheetRepo _service;

        public TimeSheetController(ITimeSheetRepo service)
        {
            _service = service;
        }

        [HttpPost("check-in")]
        public async Task<IActionResult> CheckIn(string staffID)
        {
            await _service.CheckIn(staffID);
            return Ok(new
            {
                success = true
            });
        }

        [HttpPost("check-out")]
        public async Task<IActionResult> CheckOut(string staffID)
        {
            await _service.CheckOut(staffID);
            return Ok(new
            {
                success = true
            });
        }

        [HttpGet("get-total-by-month/{month}/{year}")]
        public async Task<IActionResult> GetTotalByMonth(string staffID, int month, int year)
        {
            return Ok(new
            {
                data = await _service.GetTotalByMonth(staffID, month, year)
            });
        }

        [HttpGet("get-avg-by-month/{month}/{year}")]
        public async Task<IActionResult> GetAvgByMonth(string staffID, int month, int year)
        {
            return Ok(new
            {
                data = await _service.GetAvgByMonth(staffID, month, year)
            });
        }

        [HttpPost("re-check-in")]
        public async Task<IActionResult> ReCheckIn(string staffID, DateTime date)
        {
            try
            {
                await _service.ReCheckIn(staffID, date);
                return Ok(new {
                    success = true
                });
            }
            catch(Exception ex)
            {
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("re-check-out")]
        public async Task<IActionResult> ReCheckOut(string staffID, DateTime date)
        {
            try
            {
                await _service.ReCheckOut(staffID, date);
                return Ok(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
    }
}
