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
    public class RepositoryMenu : IRepositoryMenu
    {
        private readonly HelaTicoContext _context;
        public RepositoryMenu(HelaTicoContext context)
        {
            _context = context;
        }




        public async Task<Menu> FindByIdAsync(int id)
        {
            var @object = await _context.Menu
                .Include(m => m.IdCombo)
                .Include(m => m.IdProducto)
                    .ThenInclude(p => p.IdCategoriaNavigation)
                .FirstOrDefaultAsync(m => m.IdMenu == id);

            return @object;
        }





        public async Task<ICollection<Menu>> ListAsync()
        {
            var collection = await _context.Menu
                .Include(m => m.IdCombo)
                .Include(m => m.IdProducto)
                    .ThenInclude(p => p.IdCategoriaNavigation)
                .OrderByDescending(m => m.FechaInicio) 
                .ToListAsync();

            return collection;
        }



        public async Task<ICollection<Menu>> GetMenusDisponiblesAsync()
        {
            var hoy = DateTime.Now;

            return await _context.Menu
                .Include(m => m.IdProducto)
                    .ThenInclude(p => p.IdCategoriaNavigation)
                .Include(m => m.IdCombo)

                .Where(m => m.EstadoMenu == 1
                         && m.FechaInicio <= hoy
                         && m.FechaFinal >= hoy)

                .OrderByDescending(m => m.FechaInicio)
                .ToListAsync(); 
        }




    }
}
