using AppointmentHospital.Server.Services.ForMeet;
using AppointmentHospital.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentHospital.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetController : ControllerBase
    {
        private readonly IMeetService _meetService;
        public MeetController(IMeetService meetService)
        {
            _meetService = meetService;
        }

        [HttpPost("createMeet"),Authorize]
        public async Task<ActionResult<ServiceResponse<Meet>>> CreateMeet(Meet meet)
        {
            var result = await _meetService.CreateMeet(meet);
            return Ok(result);
        }

        [HttpPut("cancelMeet/{meetId}"),Authorize]
        public async Task<ActionResult<ServiceResponse<Meet>>> CancelMeet([FromRoute]int meetId)
        {
            var result = await _meetService.CancelMeet(meetId);
            return Ok(result);
        }

        [HttpGet,Authorize]
        public async Task<ActionResult<ServiceResponse<List<Meet>>>> GetMyMeet()
        {
            var result = await _meetService.GetMeetById();
            return Ok(result);
        }

    }
}
//Task<ServiceResponse<Meet>> CreateMeet(Meet meet);
//Task<ServiceResponse<Meet>> CancelMeet(int id);
//Task<ServiceResponse<List<Meet>>> GetMeetById();