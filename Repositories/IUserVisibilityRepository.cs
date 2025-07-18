using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IUserVisibilityRepository
    {
        Task<bool> CanSeeAsync(string ownerUserId, string allowedUserId);
        Task AddAsync(UserVisibility visibility);
        Task RemoveAsync(UserVisibility visibility);
        Task<UserVisibility> GetAsync(string ownerUserId, string allowedUserId);
        Task SaveChangesAsync();
    }
}