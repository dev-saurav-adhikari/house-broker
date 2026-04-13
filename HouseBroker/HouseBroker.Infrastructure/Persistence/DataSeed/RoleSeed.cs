using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HouseBroker.Infrastructure.Persistence.DataSeed;

public static class RoleSeed
{
    public static void SeedRole(this ModelBuilder builder)
    {
        builder.Entity<IdentityRole<long>>().HasData(
            new IdentityRole<long>() { Id = 1, Name = "Broker", NormalizedName = "BROKER", ConcurrencyStamp = "cdfbd862-6180-434e-892e-e1658f8b4c64"},
            new IdentityRole<long>() { Id = 2, Name = "HouseSeeker", NormalizedName = "HOUSESEEKER", ConcurrencyStamp ="4d585e78-725b-455b-bde4-4363f905e576" });
    }
}