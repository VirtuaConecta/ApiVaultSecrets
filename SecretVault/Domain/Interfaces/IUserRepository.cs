using SecretVault.Domain.Entities;

namespace SecretVault.Domain.Interfaces
{
    public interface IUserRepository
    {

        Task<User?> GetByUsernameAndPasswordAsync(string username, string password);
        Task<User?> GetByUsernameAsync(string username);
        Task<User> AddAsync(User user);
        Task UpdateAsync(User user);
        Task<IEnumerable<User>> GetAllAsync();
    }
}
