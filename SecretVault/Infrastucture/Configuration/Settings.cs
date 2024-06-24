using SecretVault.Domain.Entities;

namespace SecretVault.Infrastucture.Configuration
{
    public class Settings
    {

        public static string ConnectionString { get; set; } = String.Empty;
        public static string EncriptyKey { get; set; } = String.Empty;
        public static string Secret { get; set; } = String.Empty;


        public static User AdminInt = new User
        {
            Username = "admin",
            Password = "3CB4B692CDAE4539AAE29CA5F2C90918",
            Role = "Admin"
        };
    }
}
