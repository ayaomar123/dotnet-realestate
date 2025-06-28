using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Realestate.Entities;

namespace Realestate.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

            if (context.Categories.Any() ||
                context.Cities.Any() ||
                context.Districts.Any() ||
                context.Types.Any() ||
                context.PropertyTypes.Any() ||
                context.Users.Any() ||
                context.Statuses.Any())
            {
                return;
            }

            context.Categories.AddRange(
                new Category { NameAr = "بيع", NameEn = "Buy"},
                new Category { NameAr = "إيحار", NameEn = "Rent"}
            );

            context.Cities.AddRange(
                new City { NameAr = "الرياض", NameEn = "Riyadh" },
                new City { NameAr = "جدة", NameEn = "Jeddah" }
            );

            context.Districts.AddRange(
                new District { NameAr = "السلام", NameEn = "Alsalam",CityId = 1 },
                new District { NameAr = "القدس", NameEn = "ALquds", CityId = 1 }
            );

            context.Types.AddRange(
                new MyType { NameAr = "عوائل", NameEn = "Families" },
                new MyType { NameAr = "عزاب", NameEn = "Singles" }
            );

            context.PropertyTypes.AddRange(
                new PropertyType { NameAr = "شقق", NameEn = "Apartments" },
                new PropertyType { NameAr = "فلل", NameEn = "Villas" }
            );

            context.Statuses.AddRange(
                new Status { NameAr = "متاح", NameEn = "Available",Color = "white", BackgroundColor="green",Sort = 1},
                new Status { NameAr = "مؤجر", NameEn = "Rent", Color = "white", BackgroundColor = "red", Sort = 2 }
            );

            context.Users.AddRange(
                new User { 
                    FNameAr = "مسؤول",
                    FNameEn= "Super",
                    LNameAr="النظام",
                    LNameEn= "Admin",
                    Email = "Admin@gmail.com",
                    PasswordHash = new PasswordHasher<User>().HashPassword(null!, "Admin123!"),
                    PhoneNumber = "1234567890",
                    Role = "Admin",
                    UserName = "Admin@gmail.com"    
                }
            );

            context.SaveChanges();
        }
    }
}
