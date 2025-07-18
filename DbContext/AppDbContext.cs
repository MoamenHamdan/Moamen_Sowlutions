using Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DbContext
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserLocation> UserLocations { get; set; }
        public DbSet<UserVisibility> UserVisibilities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserVisibility>()
                .HasOne(uv => uv.OwnerUser)
                .WithMany()
                .HasForeignKey(uv => uv.OwnerUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserVisibility>()
                .HasOne(uv => uv.AllowedUser)
                .WithMany()
                .HasForeignKey(uv => uv.AllowedUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}