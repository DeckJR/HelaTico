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
            {
                query = query.Where(p => p.EstadoPedido == estadoPedido.Value);
            }

            return query;
        }
    }
}