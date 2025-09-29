using Core.Iottu.Domain.Entities;
using Core.Iottu.Domain.Interfaces;
using Infrastructure.Iottu.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Iottu.Persistence.Repositories
{
    public class MotoRepository : IMotoRepository
    {
        private readonly IottuDbContext _context;

        public MotoRepository(IottuDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Moto>> GetAllAsync(int page, int pageSize)
        {
            return await _context.Motos
                .Include(m => m.Tag)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Moto?> GetByIdAsync(Guid id)
        {
            return await _context.Motos
                .Include(m => m.Tag)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddAsync(Moto moto)
        {
            await _context.Motos.AddAsync(moto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Moto moto)
        {
            _context.Motos.Update(moto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto != null)
            {
                _context.Motos.Remove(moto);
                await _context.SaveChangesAsync();
            }
        }
    }
}
