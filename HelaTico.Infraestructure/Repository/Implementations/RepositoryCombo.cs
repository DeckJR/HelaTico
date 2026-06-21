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
    public class RepositoryCombo : IRepositoryCombo
    {
        private readonly HelaTicoContext _context;

        public RepositoryCombo(HelaTicoContext context)
        {
            _context = context;
        }

        public async Task<Combo> FindByIdAsync(int id)
        {
            var @object = await _context.Combo
                    .Include(c => c.ComboProducto)
                        .ThenInclude(cp => cp.IdProductoNavigation)
                    .FirstOrDefaultAsync(c => c.IdCombo == id);
            return @object!;
        }

        public async Task<ICollection<Combo>> ListAsync()
        {
            var collection = await _context.Combo
                    .Include(c => c.ComboProducto)
                        .ThenInclude(cp => cp.IdProductoNavigation)
                    .ToListAsync();
            return collection;
        }
    }
}
