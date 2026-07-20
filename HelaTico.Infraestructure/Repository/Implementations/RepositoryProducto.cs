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
        public async Task<int> AddAsync(Producto entity, int[] idsIngredientes)
        {
            //Relación de muchos a muchos solo con llave primaria compuesta
            var ingredientes = await getIngredientes(idsIngredientes);
            entity.IdIngrediente = ingredientes;
            await _context.Set<Producto>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdProducto;
        }
        public async Task UpdateAsync(Producto entity, int[] idsIngredientes)
        {
            var producto = await _context.Producto
                .Include(p => p.IdIngrediente)
                .FirstOrDefaultAsync(p => p.IdProducto == entity.IdProducto);
            if (producto == null)
            {
                throw new Exception("Producto no encontrado");
            }
            producto.Nombre = entity.Nombre;
            producto.Descripcion = entity.Descripcion;
            producto.Precio = entity.Precio;
            producto.IdCategoria = entity.IdCategoria;
            producto.EstadoProducto = entity.EstadoProducto;
            producto.Imagen = entity.Imagen;
            producto.IdIngrediente.Clear();
            var nuevosIngredientes = await getIngredientes(idsIngredientes);
            foreach (var ingrediente in nuevosIngredientes)
            {
                producto.IdIngrediente.Add(ingrediente);
            }
            await _context.SaveChangesAsync();
        }
        private async Task<ICollection<Ingrediente>> getIngredientes(int[] idsIngredientes)
        {
            // Buscar o crear ingredientes
            var ingredientes = await _context.Set<Ingrediente>()
                .Where(i => idsIngredientes.Contains(i.IdIngrediente))
                .ToListAsync();
            return ingredientes;
        }
        public async Task<bool> ExisteNombreAsync(string nombre, int id)
        {
            nombre = nombre.Trim().ToUpper();
            return await _context.Producto
                .AnyAsync(p => p.Nombre.Trim().ToUpper() == nombre && p.IdProducto != id);
        }
    }
}