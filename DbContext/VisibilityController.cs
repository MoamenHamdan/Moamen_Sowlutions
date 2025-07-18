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
    public class VisibilityController : ControllerBase
    {
        private readonly DbContext.AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public VisibilityController(DbContext.AppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpPost("allow")]
        public async Task<IActionResult> Allow(VisibilityDto dto)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name);
            if (await _db.UserVisibilities.AnyAsync(v => v.OwnerUserId == ownerId && v.AllowedUserId == dto.UserId))
                return BadRequest("Already allowed");
            _db.UserVisibilities.Add(new UserVisibility { OwnerUserId = ownerId, AllowedUserId = dto.UserId });
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove(VisibilityDto dto)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name);
            var vis = await _db.UserVisibilities.FirstOrDefaultAsync(v => v.OwnerUserId == ownerId && v.AllowedUserId == dto.UserId);
            if (vis == null) return NotFound();
            _db.UserVisibilities.Remove(vis);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}