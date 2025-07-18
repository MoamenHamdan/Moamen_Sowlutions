using DTO;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Services;

namespace Moamen_Sowlutions.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("Check your profile")]
        public async Task<ActionResult<ProfileDto>> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name);
            var profile = await _profileService.GetProfileAsync(userId);
            if (profile == null) return NotFound();
            return profile;
        }

        [HttpPut("Update your profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name);
            await _profileService.UpdateProfileAsync(userId, dto);
            return Ok();
        }
    }
}