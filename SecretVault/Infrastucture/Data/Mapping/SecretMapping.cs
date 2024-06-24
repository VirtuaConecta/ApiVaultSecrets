using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretVault.Domain.Entities;

namespace SecretVault.Infrastucture.Data.Mapping
{
    public class SecretMapping:IEntityTypeConfiguration<Secret>
    {
        public void Configure(EntityTypeBuilder<Secret> builder)
        {
            builder.ToTable("Secrets");
            builder.HasKey(t => t.Id);
            builder.Property(x => x.Key)
                .IsRequired().HasColumnType("TEXT");
            builder.Property(x => x.EncryptedValue).
                IsRequired().HasColumnType("TEXT");
            
        }
    }
}
