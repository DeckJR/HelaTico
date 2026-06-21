using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HelaTico.Infraestructure.Data;
using HelaTico.Infraestructure.Models;
using HelaTico.Infraestructure.Repository.Interfaces;

namespace HelaTico.Infraestructure.Repository.Implementations
{
    public class RepositoryPreparacion : IRepositoryPreparacion
    {
        private readonly HelaTicoContext _context;

        public RepositoryPreparacion(HelaTicoContext context)
        {
            _context = context;
        }

        public async Task<Producto> FindByIdAsync(int idProducto)
        {
            var @object = await _context.Producto
                    .Include(p => p.Preparacion)
                        .ThenInclude(prep => prep.IdEstacionNavigation)
                    .FirstOrDefaultAsync(p => p.IdProducto == idProducto);
            return @object!;
        }

        public async Task<ICollection<Producto>> ListAsync()
        {
            var collection = await _context.Producto
                    .Include(p => p.Preparacion)
                        .ThenInclude(prep => prep.IdEstacionNavigation)
                    .Where(p => p.Preparacion.Any())
                    .ToListAsync();
            return collection;
        }
    }
}
