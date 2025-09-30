using Core.Iottu.Domain.Entities;

namespace Core.Iottu.Domain.Interfaces
{
    public interface IPatioRepository
    {
        Task<IEnumerable<Patio>> GetAllAsync(int page, int pageSize);
        Task<Patio?> GetByIdAsync(Guid id);
        Task AddAsync(Patio patio);
        Task UpdateAsync(Patio patio);
        Task DeleteAsync(Guid id);
    }
}


