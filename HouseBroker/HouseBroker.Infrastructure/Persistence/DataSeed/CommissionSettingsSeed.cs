using HouseBroker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HouseBroker.Infrastructure.Persistence.DataSeed;

public static class CommissionSettingsSeed
{
    public static void SeedCommissionSettings(this ModelBuilder builder)
    {
        var seedDate = new DateTimeOffset(new DateTime(2026, 4, 12), TimeSpan.Zero);
        builder.Entity<CommissionSetting>().HasData(
            new CommissionSetting { Id = 1, MinimumAmount = 0, MaximumAmount = 50_00_000, Rate = 2,CreatedOn = seedDate},
            new CommissionSetting { Id = 2, MinimumAmount = 50_00_000, MaximumAmount = 1_00_00_000, Rate = 1.75M ,CreatedOn = seedDate},
            new CommissionSetting { Id = 3, MinimumAmount = 1_00_00_000, MaximumAmount = 0, Rate = 1.5M ,CreatedOn = seedDate});
    }
}