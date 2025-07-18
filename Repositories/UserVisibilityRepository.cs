using Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DbContext;

namespace Repositories
{
    public class UserVisibilityRepository : IUserVisibilityRepository
    {
        private readonly AppDbContext _db;
        public UserVisibilityRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<bool> CanSeeAsync(string ownerUserId, string allowedUserId)
        {
            return await _db.UserVisibilities.AnyAsync(v => v.OwnerUserId == ownerUserId && v.AllowedUserId == allowedUserId);
        }
        public async Task AddAsync(UserVisibility visibility)
        {
            await _db.UserVisibilities.AddAsync(visibility);
        }
        public async Task RemoveAsync(UserVisibility visibility)
        {
            _db.UserVisibilities.Remove(visibility);
        }
        public async Task<UserVisibility> GetAsync(string ownerUserId, string allowedUserId)
        {
            return await _db.UserVisibilities.FirstOrDefaultAsync(v => v.OwnerUserId == ownerUserId && v.AllowedUserId == allowedUserId);
        }
        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}