using HelaTico.Infraestructure.Data;
using HelaTico.Infraestructure.Models;
using HelaTico.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelaTico.Infraestructure.Repository.Implementations
{
    public class RepositoryUsuario : IRepositoryUsuario
    {
        private readonly HelaTicoContext _context;

        public RepositoryUsuario(HelaTicoContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> FindByCorreoAsync(string correo)
        {
            return await _context.Usuario
                .Include(u => u.IdRolUsuarioNavigation)
                .FirstOrDefaultAsync(u => u.Correo == correo);
        }

        public async Task<Usuario?> FindByIdAsync(int id)
        {
            return await _context.Usuario
                .Include(u => u.IdRolUsuarioNavigation)
                .FirstOrDefaultAsync(u => u.IdUsuario == id);
        }

        public async Task<List<Usuario>> SearchClientesAsync(string nombre)
        {
            nombre = (nombre ?? string.Empty).Trim().ToLower();

            var query = _context.Usuario.Include(u => u.IdRolUsuarioNavigation)
                .Where(u => u.IdRolUsuarioNavigation.Descripcion == "Cliente" && u.EstadoUsuario == 1);

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                query = query.Where(u => u.Nombre.ToLower().Contains(nombre) || u.Apellido1.ToLower().Contains(nombre) || u.Correo.ToLower().Contains(nombre));
            }

            return await query.OrderBy(u => u.Nombre).ThenBy(u => u.Apellido1).Take(20).ToListAsync();
        }

        public async Task<List<Usuario>> GetAllAsync()
        {
            return await _context.Usuario
                .Include(u => u.IdRolUsuarioNavigation)
                .OrderBy(u => u.IdRolUsuarioNavigation.Descripcion)
                .ThenBy(u => u.Nombre)
                .ToListAsync();
        }

        public async Task<bool> ExistsByCorreoAsync(string correo)
        {
            return await _context.Usuario.AnyAsync(u => u.Correo == correo);
        }

        public async Task<int> CreateAsync(Usuario usuario)
        {
            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario.IdUsuario;
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            _context.Usuario.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<List<RolUsuario>> GetRolesAsync()
        {
            return await _context.RolUsuario.ToListAsync();
        }
    }
}