using DTO;
using Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ProfileDto> GetProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;
            return new ProfileDto { Id = user.Id, Email = user.Email, Name = user.Name };
        }
        public async Task UpdateProfileAsync(string userId, ProfileDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return;
            user.Name = dto.Name;
            user.Email = dto.Email;
            user.UserName = dto.Email;
            await _userManager.UpdateAsync(user);
        }
    }
}