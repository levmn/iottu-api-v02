using Core.Iottu.Domain.Entities;

namespace Core.Iottu.Domain.Interfaces
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync(int page, int pageSize);
        Task<Tag?> GetByIdAsync(Guid id);
        Task AddAsync(Tag tag);
        Task UpdateAsync(Tag tag);
        Task DeleteAsync(Guid id);
    }
}
