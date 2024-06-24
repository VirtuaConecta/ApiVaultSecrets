using Microsoft.EntityFrameworkCore;
using SecretVault.Domain.Entities;
using System.Reflection;

namespace SecretVault.Infrastucture.Data.Context
{
    public class SecretVaultContext : DbContext
    {
        public SecretVaultContext(DbContextOptions<SecretVaultContext> options)
          : base(options) { }

        public DbSet<Secret> Secrets { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!; // null! informa ao compilador que este item não será nulo
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // busca os mappings todos arquivos que implemente IEntityTypeConfiguration no assembly em execução  
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
