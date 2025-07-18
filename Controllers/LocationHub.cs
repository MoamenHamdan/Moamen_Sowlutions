using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Moamen_Sowlutions.Controllers
{
    [Authorize]
    public class LocationHub : Hub
    {
        public async Task SendLocation(string userId, double latitude, double longitude)
        {
            // Broadcast to all clients (in real app, filter by allowed users)
            await Clients.All.SendAsync("ReceiveLocation", userId, latitude, longitude);
        }
    }
}