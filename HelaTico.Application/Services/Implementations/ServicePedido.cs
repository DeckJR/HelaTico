using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelaTico.Application.DTOs;
using HelaTico.Application.Enums;
using HelaTico.Application.Services.Interfaces;
using HelaTico.Infraestructure.Repository.Interfaces;

namespace HelaTico.Application.Services.Implementations
{
    public class ServicePedido : IServicePedido
    {
        private readonly IRepositoryPedido _repository;

        public ServicePedido(IRepositoryPedido repository)
        {
            _repository = repository;
        }

        public async Task<List<PedidoListaDTO>> ObtenerHistorialAsync(
            int idUsuarioLogueado, bool esCliente, DateOnly? fecha, int? estadoPedido)
        {
            var pedidos = esCliente
                ? await _repository.GetHistorialClienteAsync(idUsuarioLogueado, fecha, estadoPedido)
                : await _repository.GetHistorialTodosAsync(fecha, estadoPedido);

            return pedidos.Select(p => new PedidoListaDTO
            {
                IdPedido = p.IdPedido,
                Fecha = p.Fecha,
                NombreCliente = $"{p.IdClienteNavigation.Nombre} {p.IdClienteNavigation.Apellido1}",
                NombreEmpleado = $"{p.IdEmpleadoNavigation.Nombre} {p.IdEmpleadoNavigation.Apellido1}",
                EstadoPedido = p.EstadoPedido,
                EstadoPedidoTexto = ((EstadoPedido)p.EstadoPedido).ToString(),
                Total = p.Total,
                CantidadLineas = p.DetallePedido.Count
            }).ToList();
        }
    }
}