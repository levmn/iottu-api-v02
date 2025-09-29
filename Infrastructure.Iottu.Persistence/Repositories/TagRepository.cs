using Core.Iottu.Domain.Entities;
using Core.Iottu.Domain.Interfaces;
using Infrastructure.Iottu.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Iottu.Persistence.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly IottuDbContext _context;

        public TagRepository(IottuDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync(int page, int pageSize)
        {
            return await _context.Tags
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Tag?> GetByIdAsync(Guid id)
        {
            return await _context.Tags.FindAsync(id);
        }

        public async Task AddAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tag tag)
        {
            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag != null)
            {
                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
            }
        }
    }
}
