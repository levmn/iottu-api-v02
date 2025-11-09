using Core.Iottu.Domain.Entities;

namespace Core.Iottu.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByUsernameAsync(string username);
    Task AddAsync(Usuario usuario);
    Task<bool> DeleteAsync(Guid id);
    Task<IEnumerable<Usuario>> GetAllAsync(int page, int pageSize);
    Task<int> CountAsync();
    Task<Usuario?> GetByIdAsync(Guid id);
}
