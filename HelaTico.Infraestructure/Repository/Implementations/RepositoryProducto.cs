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
    public class RepositoryProducto : IRepositoryProducto
    {
        private readonly HelaTicoContext _context;
        public RepositoryProducto(HelaTicoContext context)
        {
            _context = context;
        }

        public async Task<Producto> FindByIdAsync(int id)
        {
            var @object = await _context.Producto
                    .Include(p => p.IdIngrediente)
                    .Include(p => p.IdCategoriaNavigation)
                    .FirstOrDefaultAsync(p => p.IdProducto == id);

            return @object!;
        }

        public async Task<ICollection<Producto>> ListAsync()
        {
            var collection = await _context.Producto
                    .Include(p => p.IdIngrediente)
                    .Include(p => p.IdCategoriaNavigation)
                    .ToListAsync();
            return collection;
        }
    }
}
