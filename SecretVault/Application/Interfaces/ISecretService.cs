using SecretVault.Domain.Entities;

namespace SecretVault.Application.Interfaces
{
    public interface ISecretService
    {
        Task<Secret> GetSecretAsync(string key);
        Task<Secret> CreateSecretAsync(Secret secret);
        Task<IEnumerable<Secret>> GetAllSecrets();
    }
}