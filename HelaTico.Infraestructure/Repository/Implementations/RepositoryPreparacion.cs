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

        public async Task AddPreparacionAsync(Preparacion preparacion)
        {
            await _context.Preparacion.AddAsync(preparacion);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByProductoAsync(int idProducto)
        {
            var pasos = _context.Preparacion
                .Where(p => p.IdProducto == idProducto);
            _context.Preparacion.RemoveRange(pasos);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Preparacion>> GetByProductoAsync(int idProducto)
        {
            return await _context.Preparacion.Include(p =>p.IdEstacionNavigation)
                .Where(p =>p.IdProducto== idProducto).OrderBy(p =>p.Orden).ToListAsync();
        }
    }
}
