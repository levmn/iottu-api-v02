using Shared.Iottu.Contracts.DTOs;

namespace Core.Iottu.Application.Services
{
    public interface IAntenaService
    {
        Task<IEnumerable<AntenaDto>> GetAllAsync(int page, int pageSize);
        Task<AntenaDto?> GetByIdAsync(Guid id);
        Task<AntenaDto> CreateAsync(CreateAntenaDto dto);
        Task<AntenaDto?> UpdateAsync(Guid id, UpdateAntenaDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
