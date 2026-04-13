using HouseBroker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HouseBroker.Infrastructure.Persistence.DataSeed;

public static class AddressSeed
{
    public static void SeedProvinceData(this ModelBuilder builder)
    {
        var seedDate = new DateTimeOffset(new DateTime(2026, 4, 12), TimeSpan.Zero);
        builder.Entity<Province>().HasData(
            new Province { Id = 1, Name = "Koshi Province", CreatedOn = seedDate },
            new Province { Id = 2, Name = "Madhesh Province", CreatedOn = seedDate },
            new Province { Id = 3, Name = "Bagmati Province", CreatedOn = seedDate },
            new Province { Id = 4, Name = "Gandaki Province", CreatedOn = seedDate },
            new Province { Id = 5, Name = "Lumbini Province", CreatedOn = seedDate },
            new Province { Id = 6, Name = "Karnali Province", CreatedOn = seedDate },
            new Province { Id = 7, Name = "Sudurpashchim Province",CreatedOn = seedDate }
        );
    }

    public static void SeedDistrictData(this ModelBuilder builder)
    {
        var seedDate = new DateTimeOffset(new DateTime(2026, 4, 12), TimeSpan.Zero);
        builder.Entity<District>().HasData(
            new District { Id = 2, Name = "Panchthar", ProvinceId = 1, CreatedOn = seedDate },
            new District { Id = 1, Name = "Taplejung", ProvinceId = 1, CreatedOn = seedDate },
            new District { Id = 3, Name = "Ilam", ProvinceId = 1, CreatedOn = seedDate },
            new District { Id = 4, Name = "Jhapa", ProvinceId = 1, CreatedOn = seedDate },
            new District { Id = 5, Name = "Morang", ProvinceId = 1, CreatedOn = seedDate },
            new District { Id = 6, Name = "Sunsari", ProvinceId = 1, CreatedOn = seedDate },
            new District { Id = 7, Name = "Dhankuta", ProvinceId = 1, CreatedOn = seedDate },
            new District { Id = 8, Name = "Terhathum", ProvinceId = 1, CreatedOn = seedDate },
            new District { Id = 9, Name = "Sankhuwasabha", ProvinceId = 1, CreatedOn = seedDate },
            new District { Id = 10, Name = "Bhojpur", ProvinceId = 1, CreatedOn = seedDate },
            new District { Id = 11, Name = "Solukhumbu", ProvinceId = 1, CreatedOn = seedDate },
            new District { Id = 12, Name = "Okhaldhunga", ProvinceId = 1, CreatedOn = seedDate },
            new District { Id = 13, Name = "Khotang", ProvinceId = 1, CreatedOn = seedDate },
            new District { Id = 14, Name = "Udayapur", ProvinceId = 1, CreatedOn = seedDate },
            new District { Id = 15, Name = "Saptari", ProvinceId = 2, CreatedOn = seedDate },
            new District { Id = 16, Name = "Siraha", ProvinceId = 2, CreatedOn = seedDate },
            new District { Id = 17, Name = "Dhanusha", ProvinceId = 2, CreatedOn = seedDate },
            new District { Id = 18, Name = "Mahottari", ProvinceId = 2, CreatedOn = seedDate },
            new District { Id = 19, Name = "Sarlahi", ProvinceId = 2, CreatedOn = seedDate },
            new District { Id = 20, Name = "Rautahat", ProvinceId = 2, CreatedOn = seedDate },
            new District { Id = 21, Name = "Bara", ProvinceId = 2, CreatedOn = seedDate },
            new District { Id = 22, Name = "Parsa", ProvinceId = 2, CreatedOn = seedDate },
            new District { Id = 23, Name = "Kathmandu", ProvinceId = 3, CreatedOn = seedDate },
            new District { Id = 24, Name = "Lalitpur", ProvinceId = 3, CreatedOn = seedDate },
            new District { Id = 25, Name = "Bhaktapur", ProvinceId = 3, CreatedOn = seedDate },
            new District { Id = 26, Name = "Chitwan", ProvinceId = 3, CreatedOn = seedDate },
            new District { Id = 27, Name = "Makwanpur", ProvinceId = 3, CreatedOn = seedDate },
            new District { Id = 28, Name = "Kavrepalanchowk", ProvinceId = 3, CreatedOn = seedDate },
            new District { Id = 29, Name = "Nuwakot", ProvinceId = 3, CreatedOn = seedDate },
            new District { Id = 30, Name = "Dhading", ProvinceId = 3, CreatedOn = seedDate },
            new District { Id = 31, Name = "Sindhupalchok", ProvinceId = 3, CreatedOn = seedDate },
            new District { Id = 32, Name = "Dolakha", ProvinceId = 3, CreatedOn = seedDate },
            new District { Id = 33, Name = "Ramechhap", ProvinceId = 3, CreatedOn = seedDate },
            new District { Id = 34, Name = "Sindhuli", ProvinceId = 3, CreatedOn = seedDate },
            new District { Id = 35, Name = "Rasuwa", ProvinceId = 3, CreatedOn = seedDate },
            new District { Id = 36, Name = "Kaski", ProvinceId = 4, CreatedOn = seedDate },
            new District { Id = 37, Name = "Tanahun", ProvinceId = 4, CreatedOn = seedDate },
            new District { Id = 38, Name = "Syangja", ProvinceId = 4, CreatedOn = seedDate },
            new District { Id = 39, Name = "Nawalpur", ProvinceId = 4, CreatedOn = seedDate },
            new District { Id = 40, Name = "Gorkha", ProvinceId = 4, CreatedOn = seedDate },
            new District { Id = 41, Name = "Baglung", ProvinceId = 4, CreatedOn = seedDate },
            new District { Id = 42, Name = "Lamjung", ProvinceId = 4, CreatedOn = seedDate },
            new District { Id = 43, Name = "Parbat", ProvinceId = 4, CreatedOn = seedDate },
            new District { Id = 44, Name = "Myagdi", ProvinceId = 4, CreatedOn = seedDate },
            new District { Id = 45, Name = "Mustang", ProvinceId = 4, CreatedOn = seedDate },
            new District { Id = 46, Name = "Manang", ProvinceId = 4, CreatedOn = seedDate },
            new District { Id = 47, Name = "Rupandehi", ProvinceId = 5, CreatedOn = seedDate },
            new District { Id = 48, Name = "Dang", ProvinceId = 5, CreatedOn = seedDate },
            new District { Id = 49, Name = "Banke", ProvinceId = 5, CreatedOn = seedDate },
            new District { Id = 50, Name = "Kapilvastu", ProvinceId = 5, CreatedOn = seedDate },
            new District { Id = 51, Name = "Bardiya", ProvinceId = 5, CreatedOn = seedDate },
            new District { Id = 52, Name = "Arghakhanchi", ProvinceId = 5, CreatedOn = seedDate },
            new District { Id = 53, Name = "Gulmi", ProvinceId = 5, CreatedOn = seedDate },
            new District { Id = 54, Name = "Palpa", ProvinceId = 5, CreatedOn = seedDate },
            new District { Id = 55, Name = "Pyuthan", ProvinceId = 5, CreatedOn = seedDate },
            new District { Id = 56, Name = "Rolpa", ProvinceId = 5, CreatedOn = seedDate },
            new District { Id = 57, Name = "Parasi", ProvinceId = 5, CreatedOn = seedDate },
            new District { Id = 58, Name = "Rukum East", ProvinceId = 5, CreatedOn = seedDate },
            new District { Id = 59, Name = "Surkhet", ProvinceId = 6, CreatedOn = seedDate },
            new District { Id = 60, Name = "Dailekh", ProvinceId = 6, CreatedOn = seedDate },
            new District { Id = 61, Name = "Salyan", ProvinceId = 6, CreatedOn = seedDate },
            new District { Id = 62, Name = "Jajarkot", ProvinceId = 6, CreatedOn = seedDate },
            new District { Id = 63, Name = "Rukum West", ProvinceId = 6, CreatedOn = seedDate },
            new District { Id = 64, Name = "Kalikot", ProvinceId = 6, CreatedOn = seedDate },
            new District { Id = 65, Name = "Jumla", ProvinceId = 6, CreatedOn = seedDate },
            new District { Id = 66, Name = "Mugu", ProvinceId = 6, CreatedOn = seedDate },
            new District { Id = 67, Name = "Dolpa", ProvinceId = 6, CreatedOn = seedDate },
            new District { Id = 68, Name = "Humla", ProvinceId = 6, CreatedOn = seedDate },
            new District { Id = 69, Name = "Kailali", ProvinceId = 7, CreatedOn = seedDate },
            new District { Id = 70, Name = "Kanchanpur", ProvinceId = 7, CreatedOn = seedDate },
            new District { Id = 71, Name = "Doti", ProvinceId = 7, CreatedOn = seedDate },
            new District { Id = 72, Name = "Baitadi", ProvinceId = 7, CreatedOn = seedDate },
            new District { Id = 73, Name = "Achham", ProvinceId = 7, CreatedOn = seedDate },
            new District { Id = 74, Name = "Bajhang", ProvinceId = 7, CreatedOn = seedDate },
            new District { Id = 75, Name = "Bajura", ProvinceId = 7, CreatedOn = seedDate },
            new District { Id = 76, Name = "Dadeldhura", ProvinceId = 7, CreatedOn = seedDate },
            new District { Id = 77, Name = "Darchula", ProvinceId = 7, CreatedOn = seedDate }
        );
    }
    
}