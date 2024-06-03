using Microsoft.AspNetCore.Mvc;
using StaffManagmentNET.Repositories;

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
    }
}
