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
    public class RepositoryTipoEntrega
        : IRepositoryTipoEntrega
    {
        private readonly HelaTicoContext _context;

        public RepositoryTipoEntrega(
            HelaTicoContext context)
        {
            _context = context;
        }

        public async Task<List<TipoEntrega>> ListAsync()
        {
            return await _context.TipoEntrega
                .OrderBy(x => x.Descripcion)
                .ToListAsync();
        }
    }
}
