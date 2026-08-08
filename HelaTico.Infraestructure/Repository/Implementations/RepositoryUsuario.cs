using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}