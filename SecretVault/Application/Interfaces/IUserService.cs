using SecretVault.Domain.Entities;

namespace SecretVault.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<User> AuthenticateUserExist(string username);
        Task<User> CreateUser(User user);
        Task<User> UpdateUserPassword(string username, string newPassword);
        Task<IEnumerable<User>> GetAllUsers();
    }
}
