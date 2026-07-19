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
    public class RepositoryCategoria : IRepositoryCategoria
    {
        private readonly HelaTicoContext _context;
        public RepositoryCategoria(HelaTicoContext context)
        {
            _context = context;
        }

        public async Task<Categoria> FindByIdAsync(int id)
        {
            var @object = await _context.Set<Categoria>().FindAsync(id);
            return @object!;
        }

        public async Task<ICollection<Categoria>> ListAsync()
        {
            var collection = await _context.Set<Categoria>().AsNoTracking().ToListAsync();
            return collection;
        }
    }
}
