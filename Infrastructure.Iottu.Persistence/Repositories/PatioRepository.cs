using Core.Iottu.Domain.Entities;
using Core.Iottu.Domain.Interfaces;
using Infrastructure.Iottu.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Iottu.Persistence.Repositories
{
    public class PatioRepository : IPatioRepository
    {
        private readonly IottuDbContext _context;

        public PatioRepository(IottuDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patio>> GetAllAsync(int page, int pageSize)
        {
            return await _context.Patios
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Patios.CountAsync();
        }

        public async Task<Patio?> GetByIdAsync(Guid id)
        {
            return await _context.Patios.FindAsync(id);
        }

        public async Task AddAsync(Patio patio)
        {
            Console.WriteLine($"[DEBUG] Salvando patio: Cep = '{patio.Cep}'");
            await _context.Patios.AddAsync(patio);
            await _context.SaveChangesAsync();

            var check = await _context.Patios.FindAsync(patio.Id);
            Console.WriteLine($"[DEBUG] Patio salvo: Cep = '{check?.Cep}'");
        }

        public async Task UpdateAsync(Patio patio)
        {
            _context.Patios.Update(patio);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var p = await _context.Patios.FindAsync(id);
            if (p != null)
            {
                _context.Patios.Remove(p);
                await _context.SaveChangesAsync();
            }
        }
    }
}
