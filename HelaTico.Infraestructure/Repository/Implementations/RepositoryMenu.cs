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

        public async Task<int> AddAsync(Menu entity, int[] idsProductos, int[] idsCombos)
        {
            entity.IdProducto = await getProductos(idsProductos);
            entity.IdCombo = await getCombos(idsCombos);

            await _context.Set<Menu>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdMenu;
        }

        public async Task UpdateAsync(Menu entity, int[] idsProductos, int[] idCombos)
        {
            var menu = await _context.Menu
                .Include(m => m.IdProducto)
                .Include(m => m.IdCombo)
                .FirstOrDefaultAsync(m => m.IdMenu == entity.IdMenu);

            if (menu == null)
                throw new Exception("Menú no encontrado");

            menu.Nombre = entity.Nombre;
            menu.FechaInicio = entity.FechaInicio;
            menu.FechaFinal = entity.FechaFinal;
            menu.EstadoMenu = entity.EstadoMenu;

            menu.IdProducto.Clear();

            var productos = await getProductos(idsProductos);

            foreach (var p in productos)
                menu.IdProducto.Add(p);

            menu.IdCombo.Clear();

            var combos = await getCombos(idCombos);

            foreach (var c in combos)
                menu.IdCombo.Add(c);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteNombreAsync(string nombre, int idMenuExcluir = 0)
        {
            nombre = nombre.Trim().ToUpper();
            return await _context.Menu
                .AnyAsync(m => m.Nombre.Trim().ToUpper() == nombre && m.IdMenu != idMenuExcluir);
        }

        private async Task<ICollection<Producto>> getProductos(int[] idsProductos)
        {
            var productos = await _context.Set<Producto>()
                .Where(p => idsProductos.Contains(p.IdProducto))
                .ToListAsync();
            return productos;
        }

        private async Task<ICollection<Combo>> getCombos(int[] idsCombos)
        {
            var combos = await _context.Set<Combo>()
                .Where(c => idsCombos.Contains(c.IdCombo))
                .ToListAsync();
            return combos;
        }
    }
}