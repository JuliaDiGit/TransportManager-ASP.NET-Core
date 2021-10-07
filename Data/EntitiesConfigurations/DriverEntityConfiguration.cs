using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfigurations
{
    public class DriverEntityConfiguration : IEntityTypeConfiguration<DriverEntity>
    {
        public void Configure(EntityTypeBuilder<DriverEntity> builder)
        {
            builder.ToTable("drivers");
            builder.Property(driver => driver.Name).IsRequired().HasMaxLength(50);
            builder.Property(driver => driver.CreatedDate).HasDefaultValueSql("current_timestamp");
        }
    }
}