using TransportManager.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TransportManager.Data.EntitiesConfigurations
{
    public class VehicleEntityConfiguration : IEntityTypeConfiguration<VehicleEntity>
    {
        public void Configure(EntityTypeBuilder<VehicleEntity> builder)
        {
            builder.ToTable("vehicles");
            builder.Property(vehicle => vehicle.Model).IsRequired().HasMaxLength(80);
            builder.Property(vehicle => vehicle.GovernmentNumber).IsRequired().HasMaxLength(9);
            builder.HasIndex(vehicle => vehicle.GovernmentNumber).IsUnique();
            builder.Property(vehicle => vehicle.CreatedDate).HasDefaultValueSql("current_timestamp");
        }
    }
}