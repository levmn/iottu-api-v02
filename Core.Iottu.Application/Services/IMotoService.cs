using Shared.Iottu.Contracts.DTOs;

namespace Core.Iottu.Application.Services
{
    public interface IMotoService
    {
        Task<IEnumerable<MotoDto>> GetAllAsync(int page, int pageSize);
        Task<int> CountAsync();
        Task<MotoDto?> GetByIdAsync(Guid id);
        Task<MotoDto> CreateAsync(CreateMotoDto dto);
        Task<MotoDto?> UpdateAsync(Guid id, UpdateMotoDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
