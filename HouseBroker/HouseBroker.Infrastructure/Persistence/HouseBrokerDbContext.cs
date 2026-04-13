using HouseBroker.Domain.Entities;
using HouseBroker.Infrastructure.Persistence.DataSeed;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HouseBroker.Infrastructure.Persistence;

public class HouseBrokerDbContext(DbContextOptions<HouseBrokerDbContext> options) : IdentityDbContext<IdentityUser<long>, IdentityRole<long>, long>(options)
{
    public DbSet<Property> Properties { get; set; }
    public DbSet<CommissionSetting> CommissionSettings { get; set; }
    public DbSet<Province> Provinces { get; set; }
    public DbSet<District> Districts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.SeedRole();
        builder.SeedUser();
        builder.SeedProvinceData();
        builder.SeedDistrictData();
        builder.SeedCommissionSettings();
        
        builder.Entity<Property>()
            .HasOne(p => p.Province)
            .WithMany()
            .HasForeignKey(p => p.ProvinceId)
            .OnDelete(DeleteBehavior.Restrict); 

        builder.Entity<Property>()
            .HasOne(p => p.District)
            .WithMany()
            .HasForeignKey(p => p.DistrictId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<District>()
            .HasOne(d => d.Province)
            .WithMany()
            .HasForeignKey(d => d.ProvinceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(w => 
            w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
    }
}