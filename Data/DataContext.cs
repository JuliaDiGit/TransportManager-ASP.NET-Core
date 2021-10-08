using Data.EntitiesConfigurations;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // добавляем соотвествие, которое убирает чувствительность к регистру
            modelBuilder.HasCollation("case_insensitive_collation", 
                                      locale: "en-u-ks-primary", 
                                      provider: "icu",
                                      deterministic: false);

            modelBuilder.ApplyConfiguration(new CompanyEntityConfiguration());
            modelBuilder.ApplyConfiguration(new DriverEntityConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        }

        public DbSet<CompanyEntity> Companies { get; set; }
        public DbSet<DriverEntity> Drivers { get; set; }
        public DbSet<VehicleEntity> Vehicles { get; set; }
        public DbSet<UserEntity> Users { get; set; }
    }
}