using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HouseBroker.Infrastructure.Persistence.DataSeed;

public static class UserSeed
{
    public static void SeedUser(this ModelBuilder builder)
    {
        var hasher = new PasswordHasher<IdentityUser<long>>();
        var seeker = new IdentityUser<long>
        {
            Id = 1L,
            UserName = "seeker@example.com",
            NormalizedUserName = "SEEKER@EXAMPLE.COM",
            Email = "seeker@example.com",
            NormalizedEmail = "SEEKER@EXAMPLE.COM",
            EmailConfirmed = true,
            ConcurrencyStamp ="8bee02c1-4984-404e-9d5f-be1f98ce768a",
            SecurityStamp = "becf34a4-ca72-4e52-90cd-1bab5f7b4260"
        };
        seeker.PasswordHash = hasher.HashPassword(seeker, "Seeker123!");
        
        
        var broker = new IdentityUser<long>
        {
            Id = 2L,
            UserName = "broker@example.com",
            NormalizedUserName = "BROKER@EXAMPLE.COM",
            Email = "broker@example.com",
            NormalizedEmail = "BROKER@EXAMPLE.COM",
            EmailConfirmed = true,
            ConcurrencyStamp = "36e4f5bb-50f1-4a5d-8003-b779bcb7aff3",
            SecurityStamp = "c66e733e-4cde-480a-9339-0d0ec6cbc2ef"
        };
        broker.PasswordHash = hasher.HashPassword(broker, "Broker123!");
        
        builder.Entity<IdentityUser<long>>().HasData(seeker, broker);
        
        builder.Entity<IdentityUserRole<long>>().HasData(
            new IdentityUserRole<long> { RoleId = 1, UserId = 2 },
            new IdentityUserRole<long> { RoleId = 2, UserId = 1 }
        );
    }
}