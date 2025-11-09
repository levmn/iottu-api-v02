using Core.Iottu.Domain.Entities;
using Core.Iottu.Domain.Interfaces;
using Infrastructure.Iottu.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Iottu.Persistence.Repositories
{
    public class AntenaRepository : IAntenaRepository
    {
        private readonly IottuDbContext _context;

        public AntenaRepository(IottuDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Antena>> GetAllAsync(int page, int pageSize)
        {
            return await _context.Antenas
                .Include(a => a.Patio)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<int> CountAsync()
        {
            return await _context.Antenas.CountAsync();
        }

        public async Task<Antena?> GetByIdAsync(Guid id)
        {
            return await _context.Antenas
                .Include(a => a.Patio)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task AddAsync(Antena antena)
        {
            await _context.Antenas.AddAsync(antena);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Antena antena)
        {
            _context.Antenas.Update(antena);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var antena = await _context.Antenas.FindAsync(id);
            if (antena != null)
            {
                _context.Antenas.Remove(antena);
                await _context.SaveChangesAsync();
            }
        }
    }
}
