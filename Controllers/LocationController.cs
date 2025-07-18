using DTO;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.ComponentModel;
using Services;

namespace Moamen_Sowlutions.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        /// <summary>
        /// Send your current location (latitude, longitude) to the server.
        /// </summary>
        /// <param name="dto">Location data (latitude, longitude)</param>
        /// <returns>200 OK if stored, 400 if invalid</returns>
        [HttpPost]
        public async Task<IActionResult> PostLocation([FromBody] LocationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name);
            await _locationService.PostLocationAsync(userId, dto);
            return Ok(new { message = "Location stored successfully." });
        }

        /// <summary>
        /// Get the location history of a user you are allowed to see, with 20-second intervals.
        /// </summary>
        /// <param name="userId">The user ID whose history you want to view</param>
        /// <returns>List of location points</returns>
        [HttpGet("history/{userId}")]
        public async Task<ActionResult<IEnumerable<LocationHistoryDto>>> GetHistory(string userId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var history = await _locationService.GetHistoryAsync(currentUserId, userId);
                return Ok(history);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        [HttpPost("Stop Sharing")]
        public async Task<IActionResult> StopSharing()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name);
            await _locationService.StopSharingAsync(userId);
            return Ok();
        }
        [HttpPost("Start Sharing")]
        public async Task<IActionResult> StartSharing()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name);
            await _locationService.StartSharingAsync(userId);
            return Ok();
        }
    }
}