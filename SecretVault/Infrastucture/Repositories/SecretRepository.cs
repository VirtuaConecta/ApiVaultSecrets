using Microsoft.EntityFrameworkCore;
using SecretVault.Domain.Entities;
using SecretVault.Domain.Interfaces;
using SecretVault.Infrastucture.Data.Context;

namespace SecretVault.Infrastucture.Repositories
{
    public class SecretRepository : ISecretRepository
    {
        private readonly SecretVaultContext _context;

        public SecretRepository(SecretVaultContext context)
        {
            _context = context;
        }

        public async Task<Secret?> GetByKeyAsync(string key)
        {
            return await _context.Secrets.FirstOrDefaultAsync(s => s.Key == key) ;
        }

        public async Task AddAsync(Secret secret)
        {
            _context.Secrets.Add(secret);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Secret>> GetAllAsync()
        {
            return await _context.Secrets.ToListAsync();
        }
    }

}
