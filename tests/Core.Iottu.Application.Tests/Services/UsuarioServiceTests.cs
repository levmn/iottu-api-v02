using FluentAssertions;
using Moq;
using Core.Iottu.Application.Services;
using Core.Iottu.Domain.Entities;
using Core.Iottu.Domain.Interfaces;

namespace Core.Iottu.Application.Tests.Services
{
    public class UsuarioServiceTests
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepoMock;
        private readonly UsuarioService _usuarioService;

        public UsuarioServiceTests()
        {
            _usuarioRepoMock = new Mock<IUsuarioRepository>();
            _usuarioService = new UsuarioService(_usuarioRepoMock.Object);
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldReturnUser_WhenCredentialsAreValid()
        {
            var username = "admin";
            var password = "123456";

            var validUser = new Usuario
            {
                Username = username,
                PasswordHash = GetHashedPassword(password),
                IsActive = true
            };

            _usuarioRepoMock.Setup(r => r.GetByUsernameAsync(username))
                         .ReturnsAsync(validUser);

            var result = await _usuarioService.AuthenticateAsync(username, password);

            result.Should().NotBeNull();
            result!.Username.Should().Be(username);
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var username = "ghost";
            var password = "irrelevant";

            _usuarioRepoMock.Setup(r => r.GetByUsernameAsync(username))
                         .ReturnsAsync((Usuario?)null);

            var result = await _usuarioService.AuthenticateAsync(username, password);

            result.Should().BeNull();
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldReturnNull_WhenPasswordIsInvalid()
        {
            var username = "user";
            var password = "wrongpassword";
            var validUser = new Usuario
            {
                Username = username,
                PasswordHash = GetHashedPassword("correctpassword"),
                IsActive = true
            };

            _usuarioRepoMock.Setup(r => r.GetByUsernameAsync(username))
                         .ReturnsAsync(validUser);

            var result = await _usuarioService.AuthenticateAsync(username, password);

            result.Should().BeNull();
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldReturnNull_WhenUserIsInactive()
        {
            var username = "inactiveUser";
            var password = "123456";
            var inactiveUser = new Usuario
            {
                Username = username,
                PasswordHash = GetHashedPassword(password),
                IsActive = false
            };

            _usuarioRepoMock.Setup(r => r.GetByUsernameAsync(username))
                         .ReturnsAsync(inactiveUser);

            var result = await _usuarioService.AuthenticateAsync(username, password);

            result.Should().BeNull();
        }

        private static string GetHashedPassword(string password)
        {
            using var derive = new System.Security.Cryptography.Rfc2898DeriveBytes(
                password,
                System.Text.Encoding.UTF8.GetBytes("IottuStaticSalt"),
                10000,
                System.Security.Cryptography.HashAlgorithmName.SHA256);

            return System.Convert.ToBase64String(derive.GetBytes(32));
        }
    }
}