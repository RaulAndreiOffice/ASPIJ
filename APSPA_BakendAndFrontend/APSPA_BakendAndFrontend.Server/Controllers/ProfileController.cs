using System.Security.Claims;
using APSPA_BakendAndFrontend.Server.DTOs.Profile;
using APSPA_BakendAndFrontend.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APSPA_BakendAndFrontend.Server.Controllers
{
    [ApiController]
    [Route("api/profile")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profile;

        public ProfileController(IProfileService profile)
        {
            _profile = profile;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _profile.GetAsync(GetUserId());
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateProfileDto dto)
        {
            var result = await _profile.UpdateAsync(GetUserId(), dto);
            return Ok(result);
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
