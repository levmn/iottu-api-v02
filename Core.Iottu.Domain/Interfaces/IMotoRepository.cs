using Core.Iottu.Domain.Entities;

namespace Core.Iottu.Domain.Interfaces
{
    public interface IMotoRepository
    {
        Task<IEnumerable<Moto>> GetAllAsync(int page, int pageSize);
        Task<int> CountAsync();
        Task<Moto?> GetByIdAsync(Guid id);
        Task AddAsync(Moto moto);
        Task UpdateAsync(Moto moto);
        Task DeleteAsync(Guid id);
    }
}
