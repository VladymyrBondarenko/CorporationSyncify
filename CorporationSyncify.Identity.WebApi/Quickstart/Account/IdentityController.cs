using CorporationSyncify.Identity.WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CorporationSyncify.Identity.WebApi.Quickstart.Account
{
    [ApiController]
    [Route("api/identity")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IIdentityService _identityService;

        public IdentityController(UserManager<IdentityUser> userManager,
            IIdentityService identityService)
        {
            _userManager = userManager;
            _identityService = identityService;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateIdentity(
            [FromBody] CreateAccountViewModel model,
            CancellationToken cancellationToken)
        {
            bool success = false;

            if (model != null 
                && !string.IsNullOrWhiteSpace(model.UserName)
                && !string.IsNullOrWhiteSpace(model.Password)
                && !string.IsNullOrEmpty(model.Email))
            {
                success = await _identityService.CreateIdentityAsync(
                    model.UserName,
                    model.Email,
                    model.Password,
                    cancellationToken);
            }

            if (success)
            {
                return Ok();
            }

            return BadRequest();
        }

    }
}
