using Microsoft.AspNetCore.Mvc;
using StaffManagmentNET.Models;
using StaffManagmentNET.Repositories;
using StaffManagmentNET.ViewModels;

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
        public async Task<IActionResult> CheckIn(CheckInVM vm)
        {
            try
            {
                await _service.CheckIn(vm);
                return Ok(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("check-out")]
        public async Task<IActionResult> CheckOut(CheckInVM vm)
        {
            try
            {
                await _service.CheckOut(vm);
                return Ok(new
                {
                    success = true
                });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-total-by-month/{month}/{year}")]
        public async Task<IActionResult> GetTotalByMonth(string staffID, int month, int year)
        {
            return Ok(await _service.GetTotalByMonth(staffID, month, year));
        }

        [HttpGet("get-avg-by-month/{month}/{year}")]
        public async Task<IActionResult> GetAvgByMonth(string staffID, int month, int year)
        {
            return Ok(await _service.GetAvgByMonth(staffID, month, year));
        }

        [HttpPut("update-data")]
        public async Task<IActionResult> UpdateData(UpdateTimeSheetVM vm)
        {
            try
            {
                return Ok(await _service.UpdateData(vm.StaffID!, vm.Date!, vm.H1, vm.M1, vm.H2, vm.M2, vm.Type!, vm.WrkType!, vm.Off!));
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    ex.Message
                });
            }
        }

        [HttpGet("get-sheet-by-day")]
        public async Task<IActionResult> GetSheetByDay(string staffID, int day, int month, int year)
        {
            try
            {
                return Ok(await _service.GetTimeSheetByDay(staffID, day, month, year));
            }
            catch(Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("get-sheet-by-month")]
        public async Task<IActionResult> GetSheetsByMonth(string staffID, int month, int year)
        {
            try
            {
                return Ok(await _service.GetTimeSheetByMonth(staffID, month, year));
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("get-detail")]
        public async Task<IActionResult> GetSheetDetail(string staffID, int month, int year)
        {
            return Ok(await _service.GetSheetDetail(staffID, month, year));
        }
    }
}
