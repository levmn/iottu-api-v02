using Core.Iottu.Domain.Entities;
using Shared.Iottu.Contracts.DTOs;

namespace Core.Iottu.Application.Services
{
    public interface IUsuarioService
    {

        Task<IEnumerable<UsuarioDto>> GetAllAsync(int page, int pageSize);
        Task<int> CountAsync();
        Task<UsuarioDto?> GetByIdAsync(Guid id);
        Task<UsuarioDto> CreateAsync(CreateUsuarioDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<Usuario?> AuthenticateAsync(string username, string password);
    }
}