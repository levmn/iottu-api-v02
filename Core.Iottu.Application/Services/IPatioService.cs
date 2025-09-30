using Shared.Iottu.Contracts.DTOs;

namespace Core.Iottu.Application.Services
{
    public interface IPatioService
    {
        Task<IEnumerable<PatioDto>> GetAllAsync(int page, int pageSize);
        Task<int> CountAsync();
        Task<PatioDto?> GetByIdAsync(Guid id);
        Task<PatioDto> CreateAsync(CreatePatioDto dto);
        Task<PatioDto?> UpdateAsync(Guid id, UpdatePatioDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
