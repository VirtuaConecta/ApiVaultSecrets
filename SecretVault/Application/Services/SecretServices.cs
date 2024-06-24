
using SecretVault.Application.Interfaces;
using SecretVault.Domain.Entities;
using SecretVault.Domain.Interfaces;

namespace SecretVault.Application.Services
{
    public class SecretService : ISecretService
    {
        private readonly ISecretRepository _secretRepository;
        private readonly IEncryptionService _encryptionService;

        public SecretService(ISecretRepository secretRepository, IEncryptionService encryptionService)
        {
            _secretRepository = secretRepository;
            _encryptionService = encryptionService;
        }

        public async Task<Secret> GetSecretAsync(string key)
        {
            var secret = await _secretRepository.GetByKeyAsync(key);
            if (secret != null)
            {
                secret.EncryptedValue = _encryptionService.Decrypt(secret.EncryptedValue);
            }
            return secret;
        }

        public async Task<Secret> CreateSecretAsync(Secret secret)
        {
          
            if (secret == null || string.IsNullOrWhiteSpace(secret.Key) || string.IsNullOrWhiteSpace(secret.EncryptedValue))
            {
                throw new ArgumentException("Secret, Key, and EncryptedValue cannot be null or empty.");
            }

            var existingSecret = await GetSecretAsync(secret.Key);

            if (existingSecret != null)
            {
                throw new InvalidOperationException("A secret with the given key already exists.");
            }

            try
            {
                secret.EncryptedValue = _encryptionService.Encrypt(secret.EncryptedValue);

                await _secretRepository.AddAsync(secret);

                return secret;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while creating the secret.", ex);
            }
        }

        public async Task<IEnumerable<Secret>> GetAllSecrets()
        {
            var secrets = await _secretRepository.GetAllAsync();
            // Exclude encrypted value
            foreach (var secret in secrets)
            {
                secret.EncryptedValue = null;
            }
            return secrets;
        }

    }
}
