using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Realestate.Entities;

namespace Realestate.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories => Set<Category>();


        public DbSet<City> Cities => Set<City>();

        public DbSet<District> Districts => Set<District>();

        public DbSet<MyType> Types => Set<MyType>();

        public DbSet<PropertyType> PropertyTypes => Set<PropertyType>();
        public DbSet<Status> Statuses => Set<Status>();
        public DbSet<Item> Items => Set<Item>();
    }
}
