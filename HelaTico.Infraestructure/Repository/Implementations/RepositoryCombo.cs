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

        public async Task<int> AddAsync(Combo entity)
        {
            await _context.Combo.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdCombo;
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteComboProductosAsync(int idCombo)
        {
            var items = _context.ComboProducto
                .Where(cp => cp.IdCombo == idCombo);
            _context.ComboProducto.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        public async Task AddComboProductoAsync(ComboProducto comboProducto)
        {
            await _context.ComboProducto.AddAsync(comboProducto);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var combo = await _context.Combo.FindAsync(id);
            if (combo != null)
            {
                combo.Estado = 2; 
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Combo?>FindWithProductosAsync(int idCombo)
        {
            return await _context.Combo.Include(c =>c.ComboProducto)
                .ThenInclude(cp =>cp.IdProductoNavigation).FirstOrDefaultAsync(c =>c.IdCombo== idCombo);
        }
    }
}
