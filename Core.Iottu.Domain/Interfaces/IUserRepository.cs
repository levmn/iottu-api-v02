using Core.Iottu.Domain.Entities;

namespace Core.Iottu.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task AddAsync(User user);
    Task<bool> ExistsAsync(string username);
}
