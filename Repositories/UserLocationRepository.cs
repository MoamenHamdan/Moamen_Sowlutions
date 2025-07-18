using Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbContext;

namespace Repositories
{
    public class UserLocationRepository : IUserLocationRepository
    {
        private readonly AppDbContext _db;
        public UserLocationRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task AddAsync(UserLocation location)
        {
            await _db.UserLocations.AddAsync(location);
        }
        public async Task<IEnumerable<UserLocation>> GetByUserIdAsync(string userId)
        {
            return await _db.UserLocations.Where(l => l.UserId == userId).ToListAsync();
        }
        public async Task<IEnumerable<UserLocation>> GetActiveByUserIdAsync(string userId)
        {
            return await _db.UserLocations.Where(l => l.UserId == userId && l.IsActive).ToListAsync();
        }
        public async Task<IEnumerable<UserLocation>> GetInactiveByUserIdAsync(string userId)
        {
            return await _db.UserLocations.Where(l => l.UserId == userId && !l.IsActive).ToListAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}