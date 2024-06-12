using Microsoft.AspNetCore.Mvc;
using StaffManagmentNET.Repositories;
using StaffManagmentNET.ViewModels;

namespace StaffManagmentNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestRepo _service;

        public RequestsController(IRequestRepo service)
        {
            _service = service;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllRqst()
        {
            return Ok(await _service.GetAllRqst());
        }

        [HttpGet("get-your-request")]
        public async Task<IActionResult> GetYourRqst(string staffID)
        {
            return Ok(await _service.GetYourRequest(staffID));
        }

        [HttpGet("get-need-to-accept-request")]
        public async Task<IActionResult> GetNeedToAccept(string managerID)
        {
            return Ok(await _service.GetNeedToAcceptRequest(managerID));
        }

        [HttpGet("get-request-detail")]
        public async Task<IActionResult> GetRequestDetail(string requestID, string staffID)
        {
            return Ok(await _service.GetRequestDetail(requestID, staffID));
        }

        [HttpGet("get-req-create-detail")]
        public async Task<IActionResult> GetReqCreateDetail(string createID)
        {
            return Ok(await _service.GetReqCreateDetail(createID));
        }

        [HttpPost("create-request")]
        public async Task<IActionResult> CreateRequest(RequestCreateVM vm)
        {
            await _service.CreateRequest(vm);
            return Ok();
        }

        [HttpPost("consider-request")]
        public async Task<IActionResult> ConsiderRequest(ConsiderRequestVM vm)
        {
            await _service.ConsiderRequest(vm);
            return Ok();
        }
    }
}
