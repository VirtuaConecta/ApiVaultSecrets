using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretVault.Domain.Entities;

namespace SecretVault.Infrastucture.Data.Mapping
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
             builder.ToTable("Users");
             builder.HasKey(t => t.Id);
            builder.Property(x=>x.Username)
                .IsRequired().HasColumnType("TEXT");
            builder.Property(x=>x.Password).
                IsRequired().HasColumnType("TEXT");
            builder.Property(x => x.Role)
                .IsRequired().HasColumnType("TEXT");
        }
    }
}
