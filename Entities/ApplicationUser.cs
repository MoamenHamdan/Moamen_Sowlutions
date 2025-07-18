using Microsoft.AspNetCore.Identity;

namespace Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        // Add more profile fields as needed
    }
}