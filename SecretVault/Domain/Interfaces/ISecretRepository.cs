using SecretVault.Domain.Entities;

namespace SecretVault.Domain.Interfaces
{
    public interface ISecretRepository
    {
        Task<Secret?> GetByKeyAsync(string key);
        Task AddAsync(Secret secret);
        Task<IEnumerable<Secret>> GetAllAsync();
    }
}
