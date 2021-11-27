using Microsoft.EntityFrameworkCore;
using TransportManager.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TransportManager.Data.EntitiesConfigurations
{
    public class CompanyEntityConfiguration : IEntityTypeConfiguration<CompanyEntity>
    {
        public void Configure(EntityTypeBuilder<CompanyEntity> builder)
        {
            builder.ToTable("companies");
            builder.HasKey(company => company.CompanyId);
            builder.Property(company => company.CompanyId).ValueGeneratedNever();
            builder.Property(company => company.CompanyName).IsRequired().HasMaxLength(80);
            builder.Property(company => company.Id).ValueGeneratedOnAdd();
            builder.Property(company => company.CreatedDate).HasDefaultValueSql("current_timestamp");
        }
    }
}