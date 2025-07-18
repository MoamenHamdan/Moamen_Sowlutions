using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IUserLocationRepository
    {
        Task AddAsync(UserLocation location);
        Task<IEnumerable<UserLocation>> GetByUserIdAsync(string userId);
        Task<IEnumerable<UserLocation>> GetActiveByUserIdAsync(string userId);
        Task<IEnumerable<UserLocation>> GetInactiveByUserIdAsync(string userId);
        Task SaveChangesAsync();
    }
}