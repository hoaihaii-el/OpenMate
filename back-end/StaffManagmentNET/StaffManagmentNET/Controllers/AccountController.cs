using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StaffManagmentNET.Models;
using StaffManagmentNET.Repositories;
using StaffManagmentNET.ViewModels;

namespace StaffManagmentNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepo _service;

        public AccountController(IAccountRepo service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public async Task<IActionResult> SignIn(SigninVM signIn)
        {
            try
            {
                return Ok(new
                {
                    message = "Success",
                    data = await _service.SignIn(signIn)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            try
            {
                return Ok(new
                {
                    message = "Success",
                    data = await _service.Register(register)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM change)
        {
            try
            {
                await _service.ChangePassword(change.UserID!, change.OldPw!, change.NewPw!);
                return Ok(new
                {
                    message = "Success"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                });
            }
        }
    }
}
