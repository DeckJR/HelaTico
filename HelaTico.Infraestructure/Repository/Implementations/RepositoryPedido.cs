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
    public class RepositoryPedido : IRepositoryPedido
    {
        private readonly HelaTicoContext _context;

        public RepositoryPedido(HelaTicoContext context)
        {
            _context = context;
        }

        public async Task<List<Pedido>> GetHistorialClienteAsync(int idCliente, DateOnly? fecha, int? estadoPedido)
        {
            var query = BaseQuery().Where(p => p.IdCliente == idCliente);
            query = AplicarFiltros(query, fecha, estadoPedido);
            return await query.OrderByDescending(p => p.Fecha).ToListAsync();
        }

        public async Task<List<Pedido>> GetHistorialTodosAsync(DateOnly? fecha, int? estadoPedido)
        {
            var query = BaseQuery();
            query = AplicarFiltros(query, fecha, estadoPedido);
            return await query.OrderByDescending(p => p.Fecha).ToListAsync();
        }

        public async Task<Pedido?> FindByIdAsync(int id)
        {
            return await _context.Pedido
                .Include(p => p.IdClienteNavigation)
                .Include(p => p.IdEmpleadoNavigation)
                .Include(p => p.IdTipoEntregaNavigation)
                .Include(p => p.DetallePedido)
                    .ThenInclude(d => d.IdProductoNavigation)
                .Include(p => p.DetallePedido)
                    .ThenInclude(d => d.IdComboNavigation)
                .Include(p => p.Pago)
                .FirstOrDefaultAsync(p => p.IdPedido == id);
        }

        public async Task<int> RegistrarPedidoAsync(Pedido pedido)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.Pedido
                    .AddAsync(pedido);

                await _context
                    .SaveChangesAsync();


                // Registrar el primer estado del pedido en el historial que sería el de Pnediente de pago
                var historial = new HistorialEstadoPedido
                    {
                        IdPedido = pedido.IdPedido,
                        EstadoPedido = pedido.EstadoPedido,
                        FechaYhora =DateTime.Now
                    };

                await _context.HistorialEstadoPedido.AddAsync(historial);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return pedido.IdPedido;
            }
            catch
            {
                await transaction .RollbackAsync();
                throw;
            }
        }

        private IQueryable<Pedido> BaseQuery()
        {
            return _context.Pedido
                .Include(p => p.IdClienteNavigation)
                .Include(p => p.IdEmpleadoNavigation)
                .Include(p => p.DetallePedido);
        }

        private static IQueryable<Pedido> AplicarFiltros(IQueryable<Pedido> query, DateOnly? fecha, int? estadoPedido)
        {
            if (fecha.HasValue)
            {
                var inicio = fecha.Value.ToDateTime(TimeOnly.MinValue);
                var fin = fecha.Value.ToDateTime(TimeOnly.MaxValue);
                query = query.Where(p => p.Fecha >= inicio && p.Fecha <= fin);
            }
            if (estadoPedido.HasValue)
                query = query.Where(p => p.EstadoPedido == estadoPedido.Value);

            return query;
        }

        public Task<Pedido?>FindSimpleByIdAsync(int idPedido)
        {
            return _context.Pedido.Include(p => p.Pago).FirstOrDefaultAsync(p => p.IdPedido == idPedido);
        }

        public async Task AddPagoAsync(Pago pago)
        {
            await _context.Pago.AddAsync(pago);

            await _context.SaveChangesAsync();
        }

        public async Task CambiarEstadoAsync(int idPedido,int nuevoEstado)
        {
            var pedido =await _context.Pedido.FindAsync(idPedido);

            if (pedido == null)
            {
                throw new InvalidOperationException("Pedido no encontrado.");
            }

            pedido.EstadoPedido =nuevoEstado;

            var historial =new HistorialEstadoPedido
                {
                    IdPedido = idPedido,
                    EstadoPedido = nuevoEstado,
                    FechaYhora = DateTime.Now
                };

            await _context.HistorialEstadoPedido.AddAsync(historial);

            await _context.SaveChangesAsync();
        }
    }
}
