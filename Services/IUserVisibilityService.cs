using DTO;
using System.Threading.Tasks;

namespace Services
{
    public interface IUserVisibilityService
    {
        Task AllowAsync(string ownerId, string allowedUserId);
        Task RemoveAsync(string ownerId, string allowedUserId);
        Task<bool> CanSeeAsync(string ownerId, string allowedUserId);
    }
}