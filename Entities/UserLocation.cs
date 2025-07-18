using System;

namespace Entities
{
    public class UserLocation
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsActive { get; set; } // For real-time sharing control
        public ApplicationUser User { get; set; }
    }
}