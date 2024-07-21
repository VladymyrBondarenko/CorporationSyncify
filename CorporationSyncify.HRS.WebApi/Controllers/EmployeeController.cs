using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CorporationSyncify.HRS.WebApi.Controllers
{
    [ApiController]
    [Route("api/employees")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EmployeeController : ControllerBase
    {
        [HttpGet()]
        public IActionResult Get()
        {
            return Ok("Result");
        }
    }
}
