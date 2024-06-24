using SecretVault.Application.Interfaces;
using SecretVault.Domain.Entities;
using SecretVault.Domain.Interfaces;
using SecretVault.Infrastucture.Configuration;

namespace SecretVault.Infrastucture.Data.Inicialization
{
    public static class DataInitializer
    {


        public static async Task InitializeAsync(IUserRepository user, IEncryptionService encryptservice)
        {

            var _users = await user.GetAllAsync();

            if (!_users.Any())
            {

                await user.AddAsync(new User
                {
                    Username = "Admin",
                    Password = encryptservice.Encrypt(Settings.AdminInt.Password),
                    Role = "Admin"

                });
            }

        }

    }
}
