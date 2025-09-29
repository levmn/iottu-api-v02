using Core.Iottu.Domain.Entities;

namespace Core.Iottu.Domain.Interfaces
{
    public interface IAntenaRepository
    {
        Task<IEnumerable<Antena>> GetAllAsync(int page, int pageSize);
        Task<Antena?> GetByIdAsync(Guid id);
        Task AddAsync(Antena antena);
        Task UpdateAsync(Antena antena);
        Task DeleteAsync(Guid id);
    }
}
