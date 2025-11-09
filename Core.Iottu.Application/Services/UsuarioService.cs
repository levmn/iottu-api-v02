using Core.Iottu.Domain.Entities;
using Core.Iottu.Domain.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Shared.Iottu.Contracts.DTOs;
using System.Text;

namespace Core.Iottu.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<UsuarioDto> CreateAsync(CreateUsuarioDto dto)
        {
            var existing = await _repository.GetByUsernameAsync(dto.Username);
            if (existing != null)
                throw new InvalidOperationException("Usuário já existe.");

            var usuario = new Usuario
            {
                Username = dto.Username,
                PasswordHash = HashPassword(dto.PasswordHash),
                Role = dto.Role ?? "user",
                IsActive = true
            };

            await _repository.AddAsync(usuario);

            return new UsuarioDto
            {
                Id = usuario.Id,
                Username = usuario.Username,
                PasswordHash = usuario.PasswordHash,
                Role = usuario.Role,
                IsActive = usuario.IsActive,
                CreatedAt = usuario.CreatedAt
            };
        }

        public async Task<IEnumerable<UsuarioDto>> GetAllAsync(int page, int pageSize)
        {
            var users = await _repository.GetAllAsync(page, pageSize);
            return users.Select(u => new UsuarioDto
            {
                Id = u.Id,
                Username = u.Username,
                PasswordHash = u.PasswordHash,
                Role = u.Role,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt
            });
        }

        public async Task<int> CountAsync() => await _repository.CountAsync();

        public async Task<UsuarioDto?> GetByIdAsync(Guid id)
        {
            var usuario = await _repository.GetByIdAsync(id);
            return usuario == null ? null : new UsuarioDto
            {
                Id = usuario.Id,
                Username = usuario.Username,
                PasswordHash = usuario.PasswordHash,
                Role = usuario.Role,
                IsActive = usuario.IsActive,
                CreatedAt = usuario.CreatedAt
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<Usuario?> AuthenticateAsync(string username, string password)
        {
            var usuario = await _repository.GetByUsernameAsync(username);
            if (usuario == null || !usuario.IsActive)
                return null;

            var hashed = HashPassword(password);
            return hashed == usuario.PasswordHash ? usuario : null;
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
