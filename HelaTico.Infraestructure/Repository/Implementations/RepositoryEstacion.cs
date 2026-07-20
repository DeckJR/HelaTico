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
    public class RepositoryEstacion : IRepositoryEstacion
    {
        private readonly HelaTicoContext _context;

        public RepositoryEstacion(HelaTicoContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Estacion>> ListAsync()
        {
            return await _context.Estacion.ToListAsync();
        }
    }
}
