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
    public class RepositoryOrden: IRepositoryOrden
    {
        private readonly HelaTicoContext _context;

        public RepositoryOrden(HelaTicoContext context)
        {
            _context = context;
        }

        public async Task AddRangeAsync(IEnumerable<Orden> ordenes)
        {
            await _context.Orden.AddRangeAsync(ordenes);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Estacion>>GetEstacionesAsync()
        {
            return await _context.Estacion.OrderBy(e =>e.Descripcion).ToListAsync();
        }


        public async Task<List<Orden>>GetByEstacionAsync(int idEstacion)
        {
            return await _context.Orden.Include(o =>o.IdProductoNavigation)
                .Include(o =>o.IdEstacionNavigation).Include(o =>o.IdDetallePedidoNavigation)
                    .ThenInclude(d =>d.IdPedidoNavigation).Include(o =>o.IdDetallePedidoNavigation)
                        .ThenInclude(d =>d.IdComboNavigation).ThenInclude(c =>c.ComboProducto).Where(o =>o.IdEstacion== idEstacion).OrderBy(o =>o.EstadoOrden).ThenBy(o =>o.IdOrden).ToListAsync();
        }

        public async Task<Orden?>FindByIdAsync(int idOrden)
        {
            return await _context.Orden.Include(o =>o.IdProductoNavigation).Include(o =>o.IdEstacionNavigation)
                .Include(o =>o.IdDetallePedidoNavigation).ThenInclude(d =>d.IdPedidoNavigation).FirstOrDefaultAsync(o =>o.IdOrden== idOrden);
        }

        public async Task<List<Orden>>GetByPedidoAsync(int idPedido)
        {
            return await _context.Orden.Include(o =>o.IdDetallePedidoNavigation).Where(o =>o.IdDetallePedidoNavigation.IdPedido== idPedido).ToListAsync();
        }

        public async Task<bool>ExistenOrdenesPedidoAsync(int idPedido)
        {
            return await _context.Orden.AnyAsync(o =>o.IdDetallePedidoNavigation.IdPedido== idPedido);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
