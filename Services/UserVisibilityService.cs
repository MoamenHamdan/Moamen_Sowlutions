using DTO;
using Entities;
using Repositories;
using System.Threading.Tasks;

namespace Services
{
    public class UserVisibilityService : IUserVisibilityService
    {
        private readonly IUserVisibilityRepository _repo;
        public UserVisibilityService(IUserVisibilityRepository repo)
        {
            _repo = repo;
        }
        public async Task AllowAsync(string ownerId, string allowedUserId)
        {
            if (await _repo.CanSeeAsync(ownerId, allowedUserId))
                throw new System.Exception("Already allowed");
            await _repo.AddAsync(new UserVisibility { OwnerUserId = ownerId, AllowedUserId = allowedUserId });
            await _repo.SaveChangesAsync();
        }
        public async Task RemoveAsync(string ownerId, string allowedUserId)
        {
            var vis = await _repo.GetAsync(ownerId, allowedUserId);
            if (vis == null) throw new System.Exception("Not found");
            await _repo.RemoveAsync(vis);
            await _repo.SaveChangesAsync();
        }
        public async Task<bool> CanSeeAsync(string ownerId, string allowedUserId)
        {
            return await _repo.CanSeeAsync(ownerId, allowedUserId);
        }
    }
} 