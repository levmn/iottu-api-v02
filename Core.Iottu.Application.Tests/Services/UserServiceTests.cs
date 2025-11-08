using FluentAssertions;
using Moq;
using Core.Iottu.Application.Services;
using Core.Iottu.Domain.Entities;
using Core.Iottu.Domain.Interfaces;

namespace Core.Iottu.Application.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepoMock.Object);
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldReturnUser_WhenCredentialsAreValid()
        {
            var username = "admin";
            var password = "123456";

            var validUser = new User
            {
                Username = username,
                PasswordHash = GetHashedPassword(password),
                IsActive = true
            };

            _userRepoMock.Setup(r => r.GetByUsernameAsync(username))
                         .ReturnsAsync(validUser);

            var result = await _userService.AuthenticateAsync(username, password);

            result.Should().NotBeNull();
            result!.Username.Should().Be(username);
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var username = "ghost";
            var password = "irrelevant";

            _userRepoMock.Setup(r => r.GetByUsernameAsync(username))
                         .ReturnsAsync((User?)null);

            var result = await _userService.AuthenticateAsync(username, password);

            result.Should().BeNull();
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldReturnNull_WhenPasswordIsInvalid()
        {
            var username = "user";
            var password = "wrongpassword";
            var validUser = new User
            {
                Username = username,
                PasswordHash = GetHashedPassword("correctpassword"),
                IsActive = true
            };

            _userRepoMock.Setup(r => r.GetByUsernameAsync(username))
                         .ReturnsAsync(validUser);

            var result = await _userService.AuthenticateAsync(username, password);

            result.Should().BeNull();
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldReturnNull_WhenUserIsInactive()
        {
            var username = "inactiveUser";
            var password = "123456";
            var inactiveUser = new User
            {
                Username = username,
                PasswordHash = GetHashedPassword(password),
                IsActive = false
            };

            _userRepoMock.Setup(r => r.GetByUsernameAsync(username))
                         .ReturnsAsync(inactiveUser);

            var result = await _userService.AuthenticateAsync(username, password);

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