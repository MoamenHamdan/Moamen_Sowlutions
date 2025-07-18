using DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface ILocationService
    {
        Task PostLocationAsync(string userId, LocationDto dto);
        Task<IEnumerable<LocationHistoryDto>> GetHistoryAsync(string requesterId, string userId);
        Task StartSharingAsync(string userId);
        Task StopSharingAsync(string userId);
    }
}