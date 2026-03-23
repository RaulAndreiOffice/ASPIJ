using APSPA_BakendAndFrontend.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APSPA_BakendAndFrontend.Server.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _admin;

        public AdminController(IAdminService admin)
        {
            _admin = admin;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _admin.GetAllUsersAsync();
            return Ok(result);
        }

        [HttpGet("activities")]
        public async Task<IActionResult> GetActivities()
        {
            var result = await _admin.GetAllActivitiesAsync();
            return Ok(result);
        }

        [HttpGet("predictions")]
        public async Task<IActionResult> GetPredictions()
        {
            var result = await _admin.GetAllPredictionsAsync();
            return Ok(result);
        }
    }
}
