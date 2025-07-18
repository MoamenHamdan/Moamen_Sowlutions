using DTO;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Moamen_Sowlutions.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<ProfileDto>> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            return new ProfileDto { Id = user.Id, Email = user.Email, Name = user.Name };
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            user.Name = dto.Name;
            user.Email = dto.Email;
            user.UserName = dto.Email;
            await _userManager.UpdateAsync(user);
            return Ok();
        }
    }
}