using Shared.Iottu.Contracts.DTOs;

namespace Core.Iottu.Application.Services
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetAllAsync(int page, int pageSize);
        Task<TagDto?> GetByIdAsync(Guid id);
        Task<TagDto> CreateAsync(CreateTagDto dto);
        Task<TagDto?> UpdateAsync(Guid id, UpdateTagDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
