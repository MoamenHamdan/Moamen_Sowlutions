using DTO;
using System.Threading.Tasks;

namespace Services
{
    public interface IProfileService
    {
        Task<ProfileDto> GetProfileAsync(string userId);
        Task UpdateProfileAsync(string userId, ProfileDto dto);
    }
}