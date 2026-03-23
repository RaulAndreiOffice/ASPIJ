using System.Security.Claims;
using APSPA_BakendAndFrontend.Server.DTOs.Activity;
using APSPA_BakendAndFrontend.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APSPA_BakendAndFrontend.Server.Controllers
{
    [ApiController]
    [Route("api/activities")]
    [Authorize]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityService _activities;

        public ActivitiesController(IActivityService activities)
        {
            _activities = activities;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateActivityDto dto)
        {
            var result = await _activities.CreateAsync(GetUserId(), dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMine()
        {
            var result = await _activities.GetMyActivitiesAsync(GetUserId());
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _activities.GetByIdAsync(GetUserId(), id);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateActivityDto dto)
        {
            var result = await _activities.UpdateAsync(GetUserId(), id, dto);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _activities.DeleteAsync(GetUserId(), id);
            return NoContent();
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
