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

       public DbSet<Item> Items => Set<Item>();

        public DbSet<Image> Images { get; set; }
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<City> Cities => Set<City>();
        public DbSet<District> Districts => Set<District>();
        public DbSet<MyType> Types => Set<MyType>();
        public DbSet<PropertyType> PropertyTypes => Set<PropertyType>();
        public DbSet<Status> Statuses => Set<Status>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.District)
                .WithMany()
                .HasForeignKey(i => i.DistrictId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Item>()
            .HasMany(i => i.Images)
            .WithOne(img => img.Item)
            .HasForeignKey(img => img.ItemId)
            .OnDelete(DeleteBehavior.Cascade); // عند حذف العنصر، احذف الصور أيضًا

            modelBuilder.Entity<Item>()
                .HasOne(i => i.City)
                .WithMany()
                .HasForeignKey(i => i.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.Category)
                .WithMany()
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.MyType)
                .WithMany()
                .HasForeignKey(i => i.MyTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.PropertyType)
                .WithMany()
                .HasForeignKey(i => i.PropertyTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.Status)
                .WithMany()
                .HasForeignKey(i => i.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<District>()
                .HasOne(d => d.City)
                .WithMany()
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.Restrict);

        }

    }


}
