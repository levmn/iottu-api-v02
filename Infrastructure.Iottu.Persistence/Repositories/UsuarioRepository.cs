using Core.Iottu.Domain.Entities;
using Core.Iottu.Domain.Interfaces;
using Infrastructure.Iottu.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Iottu.Persistence.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IottuDbContext _context;

        public UsuarioRepository(IottuDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _context.Usuarios.FindAsync(id);
            if (user == null) return false;

            _context.Usuarios.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Usuario?> GetByUsernameAsync(string username)
            => await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == username);

        public async Task<Usuario?> GetByIdAsync(Guid id)
            => await _context.Usuarios.FindAsync(id);

        public async Task<IEnumerable<Usuario>> GetAllAsync(int page, int pageSize)
            => await _context.Usuarios
                .OrderBy(u => u.Username)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        public async Task<int> CountAsync()
            => await _context.Usuarios.CountAsync();
    }
}
