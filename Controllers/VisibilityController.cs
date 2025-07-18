using Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DTO;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class VisibilityController : ControllerBase
    {
        private readonly IUserVisibilityService _visibilityService;

        public VisibilityController(IUserVisibilityService visibilityService)
        {
            _visibilityService = visibilityService;
        }

        [HttpPost("allow")]
        public async Task<IActionResult> Allow(VisibilityDto dto)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name);
            try
            {
                await _visibilityService.AllowAsync(ownerId, dto.UserId);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove(VisibilityDto dto)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name);
            try
            {
                await _visibilityService.RemoveAsync(ownerId, dto.UserId);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}