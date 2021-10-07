using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfigurations
{
    class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("users");
            builder.HasIndex(user => user.Login).IsUnique();
            builder.Property(user => user.Login).IsRequired().HasMaxLength(20).IsUnicode(false);
            builder.Property(user => user.Password).IsRequired().HasMaxLength(69);
            builder.Property(user => user.CreatedDate).HasDefaultValueSql("current_timestamp");
        }
    }
}
