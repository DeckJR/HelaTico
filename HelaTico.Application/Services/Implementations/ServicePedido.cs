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
                EstadoPedidoTexto = ((EstadoPedido)p.EstadoPedido).ToString().Replace("_", " "),
                Total = p.Total,
                CantidadLineas = p.DetallePedido.Count
            }).ToList();
        }

        public async Task<PedidoDetalleDTO?> ObtenerDetalleAsync(int idPedido)
        {
            var pedido = await _repository.FindByIdAsync(idPedido);
            if (pedido == null) return null;

            var pago = pedido.Pago.FirstOrDefault();

            return new PedidoDetalleDTO
            {
                IdPedido = pedido.IdPedido,
                Fecha = pedido.Fecha,
                EstadoPedidoTexto = ((EstadoPedido)pedido.EstadoPedido).ToString().Replace("_", " "),

                NombreCliente = $"{pedido.IdClienteNavigation.Nombre} {pedido.IdClienteNavigation.Apellido1} {pedido.IdClienteNavigation.Apellido2}",
                CorreoCliente = pedido.IdClienteNavigation.Correo,

                NombreEmpleado = $"{pedido.IdEmpleadoNavigation.Nombre} {pedido.IdEmpleadoNavigation.Apellido1}",

                TipoEntrega = pedido.IdTipoEntregaNavigation.Descripcion,
                DireccionEntrega = pedido.DireccionEntrega,
                CostoEnvio = pedido.CostoEnvio,

                SubTotal = pedido.SubTotal,
                Impuesto = pedido.Impuesto,
                CuotaServicio = pedido.CuotaServicio,
                Total = pedido.Total,

                Detalle = pedido.DetallePedido.Select(d => new DetallePedidoDTO
                {
                    IdDetallePedido = d.IdDetallePedido,
                    NombreItem = d.IdProductoNavigation?.Nombre
                                   ?? d.IdComboNavigation?.Nombre
                                   ?? "—",
                    TipoItem = d.IdProducto.HasValue ? "Producto" : "Combo",
                    Cantidad = d.Cantidad,
                    SubTotal = d.SubTotal,
                    Impuesto = d.Impuesto,
                    TotalLinea = d.TotalLinea,
                    Observaciones = d.Observaciones ?? string.Empty
                }).ToList(),

                Pago = pago == null ? null : new PagoDTO
                {
                    IdPago = pago.IdPago,
                    MetodoPagoTexto = ((MetodoPago)pago.MetodoPago).ToString(),
                    Monto = pago.Monto,
                    Vuelto = pago.Vuelto,
                    Fecha = pago.Fecha
                }
            };
        }
    }
}