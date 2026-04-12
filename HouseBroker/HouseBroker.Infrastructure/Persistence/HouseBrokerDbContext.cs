using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HouseBroker.Infrastructure.Persistence;

public class HouseBrokerDbContext(DbContextOptions<HouseBrokerDbContext> options) : IdentityDbContext<IdentityUser<long>, IdentityRole<long>, long>(options)
{
    
}