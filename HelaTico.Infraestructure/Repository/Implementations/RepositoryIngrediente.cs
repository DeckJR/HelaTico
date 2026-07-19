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
    public class RepositoryIngrediente : IRepositoryIngrediente
    {
        private readonly HelaTicoContext _context;
        public RepositoryIngrediente(HelaTicoContext context)
        {
            _context = context;
        }

        public async Task<Ingrediente> FindByIdAsync(int id)
        {
            var @object = await _context.Set<Ingrediente>().FindAsync(id);
            return @object!;
        }

        public async Task<ICollection<Ingrediente>> ListAsync()
        {
            var collection = await _context.Set<Ingrediente>().AsNoTracking().ToListAsync();
            return collection;
        }
    }
}
