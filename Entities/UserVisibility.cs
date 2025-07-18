namespace Entities
{
    public class UserVisibility
    {
        public int Id { get; set; }
        public string OwnerUserId { get; set; } // The user sharing their location
        public string AllowedUserId { get; set; } // The user allowed to see the location
        public ApplicationUser OwnerUser { get; set; }
        public ApplicationUser AllowedUser { get; set; }
    }
} 