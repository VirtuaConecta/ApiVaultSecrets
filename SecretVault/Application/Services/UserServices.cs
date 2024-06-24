using SecretVault.Application.Interfaces;
using SecretVault.Domain.Entities;
using SecretVault.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace SecretVault.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncryptionService _encryptionService;
        public UserService(IUserRepository userRepository, IEncryptionService encryptionService)
        {
            _userRepository = userRepository;
            _encryptionService = encryptionService;
        }

        public async Task<User> AuthenticateUserExist(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<User> Authenticate(string username, string password)
        {
            User user = new User();
            var userSelect = await _userRepository.GetByUsernameAsync(username);
            if (userSelect != null)
            {

                String passDecripted = _encryptionService.Decrypt(userSelect.Password);

                if (password == passDecripted)
                {
                    user = userSelect;


                }


            }
            return user;
        }
        public async Task<User> CreateUser(User user)
        {
            if (!IsValidPassword(user.Password))
            {
                throw new ArgumentException("Password must be alphanumeric and 32 characters long.");
            }
            user.Password = _encryptionService.Encrypt(user.Password);

            var respAddUser = await _userRepository.AddAsync(user);
            if (respAddUser != null)
                respAddUser.Password = "";
            return respAddUser;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var users = await _userRepository.GetAllAsync();
            // Exclude password from the result
            foreach (var user in users)
            {
                user.Password = null;
            }
            return users;
        }

        public async Task<User> UpdateUserPassword(string username, string newPassword)
        {
            if (!IsValidPassword(newPassword))
            {
                throw new ArgumentException("Password must be alphanumeric and 32 characters long.");
            }
            newPassword = _encryptionService.Encrypt(newPassword);
            var user = await _userRepository.GetByUsernameAsync(username);

            if (user != null)
            {
                user.Password = newPassword;
                await _userRepository.UpdateAsync(user);
                user.Password = "";
            }

            return user;
        }


        private bool IsValidPassword(string password)
        {
            return Regex.IsMatch(password, @"^[a-zA-Z0-9]{32}$");
        }

    }

    
}
 
