using DTO;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Moamen_Sowlutions.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LocationController : ControllerBase
    {
        private readonly DbContext.AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public LocationController(DbContext.AppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> PostLocation(LocationDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name);
            var location = new UserLocation
            {
                UserId = userId,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Timestamp = DateTime.UtcNow,
                IsActive = true
            };
            _db.UserLocations.Add(location);
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("history/{userId}")]
        public async Task<ActionResult<IEnumerable<LocationHistoryDto>>> GetHistory(string userId)
        {
            var canSee = await _db.UserVisibilities.AnyAsync(v => v.OwnerUserId == userId && v.AllowedUserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!canSee && userId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();
            var history = await _db.UserLocations
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();
            // Return only one every 20 seconds
            var filtered = history
                .GroupBy(l => l.Timestamp.Ticks / TimeSpan.FromSeconds(20).Ticks)
                .Select(g => g.First())
                .OrderBy(l => l.Timestamp)
                .Select(l => new LocationHistoryDto { Latitude = l.Latitude, Longitude = l.Longitude, Timestamp = l.Timestamp })
                .ToList();
            return filtered;
        }

        [HttpPost("stop")]
        public async Task<IActionResult> StopSharing()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name);
            var activeLocations = _db.UserLocations.Where(l => l.UserId == userId && l.IsActive);
            await activeLocations.ForEachAsync(l => l.IsActive = false);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}