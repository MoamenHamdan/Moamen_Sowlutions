using DTO;
using Entities;
using Microsoft.AspNetCore.SignalR;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moamen_Sowlutions.Controllers;

namespace Services
{
    public class LocationService : ILocationService
    {
        private readonly IUserLocationRepository _locationRepo;
        private readonly IUserVisibilityRepository _visibilityRepo;
        private readonly IHubContext<LocationHub> _hubContext;
        public LocationService(IUserLocationRepository locationRepo, IUserVisibilityRepository visibilityRepo, IHubContext<LocationHub> hubContext)
        {
            _locationRepo = locationRepo;
            _visibilityRepo = visibilityRepo;
            _hubContext = hubContext;
        }
        public async Task PostLocationAsync(string userId, LocationDto dto)
        {
            var location = new UserLocation
            {
                UserId = userId,
                Latitude = Math.Round(dto.Latitude, 6),
                Longitude = Math.Round(dto.Longitude, 6),
                Timestamp = DateTime.UtcNow,
                IsActive = true
            };
            await _locationRepo.AddAsync(location);
            await _locationRepo.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveLocation", userId, location.Latitude, location.Longitude, location.Timestamp);
        }
        public async Task<IEnumerable<LocationHistoryDto>> GetHistoryAsync(string requesterId, string userId)
        {
            var canSee = await _visibilityRepo.CanSeeAsync(userId, requesterId);
            if (!canSee && userId != requesterId)
                throw new UnauthorizedAccessException("You are not allowed to view this user's location history.");
            var history = await _locationRepo.GetByUserIdAsync(userId);
            var filtered = history
                .OrderByDescending(l => l.Timestamp)
                .GroupBy(l => l.Timestamp.Ticks / TimeSpan.FromSeconds(20).Ticks)
                .Select(g => g.First())
                .OrderBy(l => l.Timestamp)
                .Select(l => new LocationHistoryDto { Latitude = l.Latitude, Longitude = l.Longitude, Timestamp = l.Timestamp })
                .ToList();
            return filtered;
        }
        public async Task StartSharingAsync(string userId)
        {
            var allLocations = await _locationRepo.GetByUserIdAsync(userId);
            foreach (var loc in allLocations)
                loc.IsActive = true;
            await _locationRepo.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("UserStartedSharing", userId);
        }
        public async Task StopSharingAsync(string userId)
        {
            var activeLocations = await _locationRepo.GetActiveByUserIdAsync(userId);
            foreach (var loc in activeLocations)
                loc.IsActive = false;
            await _locationRepo.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("UserStoppedSharing", userId);
        }
    }
}