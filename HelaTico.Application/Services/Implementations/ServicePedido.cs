using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelaTico.Application.DTOs;
using HelaTico.Application.Enums;
using HelaTico.Application.Services.Interfaces;
using HelaTico.Infraestructure.Models;
using HelaTico.Infraestructure.Repository.Interfaces;

namespace HelaTico.Application.Services.Implementations
{
    public class ServicePedido : IServicePedido
    {
        private readonly IRepositoryPedido _repository;
        private readonly IRepositoryTipoEntrega _tipoEntrega;
        private readonly IRepositoryProducto _producto;
        private readonly IRepositoryCombo _combo;
        private readonly IServiceOrden _serviceOrden;

        private const decimal IVA = 0.13m;
        private const decimal COSTO_ENVIO = 2000m;

        public ServicePedido(IRepositoryPedido repository,IRepositoryTipoEntrega tipoEntrega, IRepositoryProducto producto, IRepositoryCombo combo, IServiceOrden serviceOrden)
        {
            _repository = repository;
            _tipoEntrega = tipoEntrega;
            _producto = producto;
            _combo = combo;
            _serviceOrden = serviceOrden;
        }

        public async Task<List<PedidoListaDTO>> ObtenerHistorialAsync(int idUsuarioLogueado, bool esCliente, DateOnly? fecha, int? estadoPedido)
        {
            var pedidos = esCliente
                ? await _repository.GetHistorialClienteAsync(idUsuarioLogueado, fecha, estadoPedido)
                : await _repository.GetHistorialTodosAsync(fecha, estadoPedido);

            return pedidos.Select(p => new PedidoListaDTO
            {
                IdPedido = p.IdPedido,
                Fecha = p.Fecha,
                NombreCliente = $"{p.IdClienteNavigation.Nombre} {p.IdClienteNavigation.Apellido1}",
                NombreEmpleado = p.IdEmpleadoNavigation == null ? "Pedido realizado por cliente" : $"{p.IdEmpleadoNavigation.Nombre} {p.IdEmpleadoNavigation.Apellido1}",
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

                NombreEmpleado = pedido.IdEmpleadoNavigation == null? "Pedido realizado por cliente" : $"{pedido.IdEmpleadoNavigation.Nombre} {pedido.IdEmpleadoNavigation.Apellido1}",

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

        public async Task<int> RegistrarPedidoAsync(PedidoRegistroDTO dto,int idUsuarioLogueado,bool esCliente,List<CarritoItemDTO> carrito)
        {
            if (
                carrito == null
                ||
                carrito.Count == 0
            )
            {
                throw new InvalidOperationException(
                    "El carrito está vacío.");
            }
            // Si el usuario logueado es un clientee entonces 
            // se agarra el id del usuario logueado como idCliente, de lo contrario se agarra el idCliente que viene en el dto
            int idCliente = esCliente? idUsuarioLogueado: dto.IdCliente;

            if (idCliente <= 0)
            {
                throw new InvalidOperationException(
                    "Debe seleccionar un cliente.");
            }
            // Si compra el propio cliente,
            // no hay encargado.
            int? idEmpleado = esCliente? null: idUsuarioLogueado;

            var tipos = await _tipoEntrega.ListAsync();


            var tipo =
                tipos.FirstOrDefault(
                    x =>
                        x.IdTipoEntrega
                        == dto.IdTipoEntrega);


            if (tipo == null)
            {
                throw new InvalidOperationException(
                    "Método de entrega inválido.");
            }


            bool esDomicilio =
                tipo.Descripcion
                    .Contains(
                        "domicilio",
                        StringComparison
                            .OrdinalIgnoreCase);


            if (
                esDomicilio
                &&
                string.IsNullOrWhiteSpace(
                    dto.DireccionEntrega)
            )
            {
                throw new InvalidOperationException(
                    "Debe indicar la dirección para entrega a domicilio.");
            }


            var detalles =
                new List<DetallePedido>();


            decimal subTotalPedido =
                0m;

            decimal impuestoPedido =
                0m;


            foreach (
                var item
                in carrito)
            {
                if (item.Cantidad < 1)
                {
                    throw new InvalidOperationException(
                        "Todas las cantidades deben ser mayores a cero.");
                }


                decimal precio;

                int? idProducto =
                    null;

                int? idCombo =
                    null;


                // IMPORTANTE:
                // Precio nuevamente desde BD.
                if (
                    item.Tipo.Equals(
                        "Producto",
                        StringComparison.OrdinalIgnoreCase)
                )
                {
                    var producto =
                        await _producto
                            .FindByIdAsync(
                                item.Id);


                    if (producto == null)
                    {
                        throw new InvalidOperationException(
                            $"El producto {item.Id} no existe.");
                    }


                    precio =
                        producto.Precio;

                    idProducto =
                        producto.IdProducto;
                }

                else if (
                    item.Tipo.Equals(
                        "Combo",
                        StringComparison.OrdinalIgnoreCase)
                )
                {
                    var combo =
                        await _combo
                            .FindByIdAsync(
                                item.Id);


                    if (combo == null)
                    {
                        throw new InvalidOperationException(
                            $"El combo {item.Id} no existe.");
                    }


                    precio =
                        combo.Precio;

                    idCombo =
                        combo.IdCombo;
                }

                else
                {
                    throw new InvalidOperationException(
                        "Tipo de artículo inválido.");
                }


                decimal subtotalLinea =
                    Math.Round(
                        precio
                        *
                        item.Cantidad,
                        2);


                decimal impuestoLinea =
                    Math.Round(
                        subtotalLinea
                        *
                        IVA,
                        2);


                var detalle =
                    new DetallePedido
                    {
                        IdProducto =
                            idProducto,

                        IdCombo =
                            idCombo,

                        Cantidad =
                            item.Cantidad,

                        Observaciones =
                            (item.Observaciones
                                ??
                                string.Empty)
                            .Trim(),

                        SubTotal =
                            subtotalLinea,

                        Impuesto =
                            impuestoLinea,

                        TotalLinea =
                            subtotalLinea
                            +
                            impuestoLinea
                    };


                detalles.Add(
                    detalle);


                subTotalPedido +=
                    subtotalLinea;


                impuestoPedido +=
                    impuestoLinea;
            }


            decimal costoEnvio =
                esDomicilio
                ? COSTO_ENVIO
                : 0m;


            decimal total =
                subTotalPedido
                +
                impuestoPedido
                +
                costoEnvio;


            var pedido =
                new Pedido
                {
                    IdCliente =
                        idCliente,

                    IdEmpleado =
                        idEmpleado,

                    Fecha =
                        DateTime.Now,

                    IdTipoEntrega =
                        dto.IdTipoEntrega,

                    DireccionEntrega =
                        esDomicilio
                        ? dto.DireccionEntrega!.Trim()
                        : string.Empty,

                    CostoEnvio =
                        costoEnvio,

                    SubTotal =
                        subTotalPedido,

                    Impuesto =
                        impuestoPedido,

                    CuotaServicio =
                        0m,

                    Total =
                        total,

                    // MUY IMPORTANTE:
                    EstadoPedido =
                        (int)
                        EstadoPedido
                            .PendienteDePago,

                    DetallePedido =
                        detalles
                };


            return await _repository.RegistrarPedidoAsync(pedido);
        }
        //Lo mismo que hace el enum pero hubieron errores entonces
        //lo implementamos aquí desde el servicio
        private static string TextoEstado(EstadoPedido estado)
        {
            return estado 
            switch
            {
                EstadoPedido.PendienteDePago=> "Pendiente de pago",
                EstadoPedido.Aceptada=> "Aceptada",
                EstadoPedido.Preparacion=> "Preparación",
                EstadoPedido.Procesando=> "Procesando",
                EstadoPedido.Entregada=> "Entregada",
                _=>estado.ToString()
            };
        }
        public async Task<ProcesarPagoDTO?>
    PrepararPagoAsync(int idPedido)
        {
            var pedido = await _repository.FindSimpleByIdAsync(idPedido);

            if (pedido == null)
                return null;

            if (pedido.EstadoPedido!=(int)EstadoPedido.PendienteDePago)
            {
                throw new InvalidOperationException("Este pedido ya fue pagado o no se encuentra pendiente de pago.");
            }

            if (pedido.Pago.Any())
            {
                throw new InvalidOperationException("Este pedido ya tiene un pago registrado.");
            }

            return new ProcesarPagoDTO
            {
                IdPedido = pedido.IdPedido,

                Total = pedido.Total
            };
        }
        public async Task ProcesarPagoAsync(ProcesarPagoDTO dto)
        {            
            var pedido = await _repository.FindSimpleByIdAsync(dto.IdPedido);

            if (pedido == null)
            {
                throw new InvalidOperationException("Pedido no encontrado.");
            }

            if (pedido.EstadoPedido!=(int)EstadoPedido.PendienteDePago)
            {
                throw new InvalidOperationException("El pedido ya fue procesado.");
            }

            if (pedido.Pago.Any())
            {
                throw new InvalidOperationException("Este pedido ya tiene un pago registrado.");
            }

            if (dto.MetodoPago < 1||dto.MetodoPago > 3)
            {
                throw new InvalidOperationException("Método de pago inválido.");
            }

            decimal montoPagado;
            decimal vuelto = 0m;

            if (dto.MetodoPago==(int)MetodoPago.Efectivo)
            {
                if (!dto.MontoEfectivo.HasValue)
                {
                    throw new InvalidOperationException("Debe indicar el monto recibido.");
                }

                if (dto.MontoEfectivo.Value<pedido.Total)
                {
                    throw new InvalidOperationException($"El monto recibido es insuficiente. " + $"El total es ₡{pedido.Total:N2}.");
                }

                montoPagado =dto.MontoEfectivo.Value;

                vuelto = Math.Round(montoPagado-pedido.Total,2);
            }
            else
            {
                ValidarTarjeta(dto);
                montoPagado =pedido.Total;
                vuelto =0m;
            }

            var pago =new Pago
                {
                    IdPedido =pedido.IdPedido,
                    MetodoPago =dto.MetodoPago,
                    Monto =montoPagado,
                    Vuelto = vuelto, 
                    Fecha = DateTime.Now
                };
            await _repository.AddPagoAsync(pago);

            await _repository.CambiarEstadoAsync(pedido.IdPedido,(int)EstadoPedido.Aceptada);

            await _serviceOrden.GenerarOrdenesPedidoAsync(pedido.IdPedido);
        }
        private static void ValidarTarjeta(ProcesarPagoDTO dto)
        {
            string numero =(dto.NumeroTarjeta??string.Empty).Replace(" ", "").Replace("-", "");

            if (numero.Length < 13||numero.Length > 19||!numero.All(char.IsDigit))
            {
                throw new InvalidOperationException("El número de tarjeta no es válido.");
            }

            if (string.IsNullOrWhiteSpace(dto.NombreTarjeta))
            {
                throw new InvalidOperationException("Debe indicar el nombre del titular.");
            }

            if (string.IsNullOrWhiteSpace(dto.VencimientoTarjeta)||dto.VencimientoTarjeta.Length != 5||dto.VencimientoTarjeta[2] != '/')
            {
                throw new InvalidOperationException("La fecha de vencimiento debe utilizar el formato MM/AA.");
            }

            var partes =dto.VencimientoTarjeta.Split('/');

            if (partes.Length != 2||!int.TryParse(partes[0],out int mes)||!int.TryParse(partes[1],out int anio)||mes < 1||mes > 12)
            {
                throw new InvalidOperationException("La fecha de vencimiento no es válida.");
            }

            int anioCompleto =2000 + anio;

            var ultimoDiaMes = new DateTime(anioCompleto,mes,DateTime.DaysInMonth(anioCompleto,mes));

            if (ultimoDiaMes.Date<DateTime.Today)
            {
                throw new InvalidOperationException("La tarjeta está vencida.");
            }

            string cvv =dto.CvvTarjeta??string.Empty;

            if ((cvv.Length != 3&&cvv.Length != 4)||!cvv.All(char.IsDigit))
            {
                throw new InvalidOperationException("El CVV no es válido.");
            }
        }
    }
}