using Core.Iottu.Domain.Entities;
using Core.Iottu.Domain.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace Core.Iottu.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _repository.GetByUsernameAsync(username);
            if (user == null || !user.IsActive)
                return null;

            var hashed = HashPassword(password);
            return hashed == user.PasswordHash ? user : null;
        }

        private static string HashPassword(string password)
        {
            byte[] salt = Encoding.UTF8.GetBytes("IottuStaticSalt");
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32));
            return hash;
        }
    }
}
