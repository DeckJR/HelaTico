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
    public class ServiceOrden: IServiceOrden
    {
        private readonly IRepositoryOrden _orden;
        private readonly IRepositoryPedido _pedido;
        private readonly IRepositoryPreparacion _preparacion;
        private readonly IRepositoryCombo _combo;

        public ServiceOrden(IRepositoryOrden orden,IRepositoryPedido pedido,IRepositoryPreparacion preparacion,IRepositoryCombo combo)
        {
            _orden =orden;
            _pedido =pedido;
            _preparacion =preparacion;
            _combo =combo;
        }

        public async Task GenerarOrdenesPedidoAsync(int idPedido)
        {

            if (await _orden.ExistenOrdenesPedidoAsync(idPedido))
            {
                return;
            }

            var pedido =await _pedido.FindWithDetalleAsync(idPedido);

            if (pedido == null)
            {
                throw new InvalidOperationException("Pedido no encontrado.");
            }

            if (pedido.EstadoPedido!=(int)EstadoPedido.Aceptada)
            {
                throw new InvalidOperationException("El pedido debe estar aceptado antes de generar sus órdenes de preparación.");
            }            

            var ordenes =new List<Orden>();

            foreach (var detalle in pedido.DetallePedido)
            {                
                //Sí es producto
                if (detalle.IdProducto.HasValue)
                {
                    await GenerarParaProductoAsync(ordenes,detalle,detalle.IdProducto.Value);
                }
                //Si es combo
                else if (detalle.IdCombo.HasValue)
                {
                    var combo =await _combo.FindWithProductosAsync(detalle.IdCombo.Value);

                    if (combo == null)
                    {
                        throw new InvalidOperationException($"El combo {detalle.IdCombo.Value} no existe.");
                    }

                    foreach (var comboProducto in combo.ComboProducto)
                    {
                        await GenerarParaProductoAsync(ordenes,detalle,comboProducto.IdProducto);
                    }
                }
            }

            if (!ordenes.Any())
            {
                throw new InvalidOperationException("No fue posible generar órdenes de preparación. Verifique que los productos tengan una preparación configurada.");
            }

            await _orden.AddRangeAsync(ordenes);
        }

        private async Task GenerarParaProductoAsync(List<Orden> ordenes,DetallePedido detalle,int idProducto)
        {
            var pasos =await _preparacion.GetByProductoAsync(idProducto);

            if (!pasos.Any())
            {
                throw new InvalidOperationException($"El producto {idProducto} no tiene pasos de preparación configurados.");
            }

            foreach (var paso in pasos)
            {
                ordenes.Add(
                    new Orden
                    {
                        IdDetallePedido = detalle.IdDetallePedido,
                        IdProducto = idProducto,
                        IdEstacion = paso.IdEstacion,
                        EstadoOrden = (int)EstadoOrden.Pendiente,
                        HoraInicio = null,
                        HoraFin = null
                    }
                );
            }
        }

        public async Task<List<EstacionProcesoDTO>>ObtenerEstacionesAsync()
        {
            var estaciones =await _orden.GetEstacionesAsync();

            var resultado =new List<EstacionProcesoDTO>();

            foreach (var estacion in estaciones)
            {
                var ordenes = await _orden.GetByEstacionAsync(estacion.IdEstacion);

                resultado.Add(
                    new EstacionProcesoDTO
                    {
                        IdEstacion = estacion.IdEstacion,
                        Descripcion = estacion.Descripcion,
                        OrdenesPendientes = ordenes.Count(o => o.EstadoOrden == (int)EstadoOrden.Pendiente),
                        OrdenesEnProceso = ordenes.Count(o => o.EstadoOrden == (int)EstadoOrden.EnProceso)
                    }
                );
            }

            return resultado;
        }

        public async Task<List<OrdenEstacionDTO>>ObtenerPorEstacionAsync(int idEstacion)
        {
            var ordenes =await _orden.GetByEstacionAsync(idEstacion);

            var resultado =new List<OrdenEstacionDTO>();

            foreach (var orden in ordenes)
            {
                int cantidad = orden.IdDetallePedidoNavigation.Cantidad;

                if (orden.IdDetallePedidoNavigation.IdCombo.HasValue)
                {
                    var comboProducto =orden.IdDetallePedidoNavigation.IdComboNavigation?.ComboProducto
                            .FirstOrDefault(cp =>cp.IdProducto==orden.IdProducto);

                    if (comboProducto != null)
                    {
                        cantidad =cantidad*comboProducto.CantidadProducto;
                    }
                }

                resultado.Add(
                    new OrdenEstacionDTO
                    {
                        IdOrden =orden.IdOrden,
                        IdPedido =orden.IdDetallePedidoNavigation.IdPedido,
                        IdDetallePedido =orden.IdDetallePedido,
                        IdEstacion =orden.IdEstacion,
                        Estacion =orden.IdEstacionNavigation.Descripcion,
                        IdProducto =orden.IdProducto,
                        Producto =orden.IdProductoNavigation.Nombre,
                        Cantidad =cantidad,
                        Observaciones =orden.IdDetallePedidoNavigation.Observaciones??string.Empty,
                        EstadoOrden =orden.EstadoOrden,
                        EstadoOrdenTexto =TextoEstadoOrden((EstadoOrden)orden.EstadoOrden),
                        HoraInicio =orden.HoraInicio,
                        HoraFin =orden.HoraFin
                    }
                );
            }
            return resultado;
        }

        public async Task IniciarAsync(int idOrden)
        {
            var orden =await _orden.FindByIdAsync(idOrden);

            if (orden == null)
            {
                throw new InvalidOperationException("Orden no encontrada.");
            }

            if (orden.EstadoOrden!=(int)EstadoOrden.Pendiente)
            {
                throw new InvalidOperationException("La orden ya fue iniciada o finalizada.");
            }

            await ValidarPasoAnteriorAsync(orden);

            orden.EstadoOrden = (int)EstadoOrden.EnProceso;

            orden.HoraInicio =TimeOnly.FromDateTime(DateTime.Now);

            await _orden.SaveChangesAsync();

            int idPedido =orden.IdDetallePedidoNavigation.IdPedido;            

            var pedido =await _pedido.FindSimpleByIdAsync(idPedido);

            if (pedido != null&&pedido.EstadoPedido==(int)EstadoPedido.Aceptada)
            {
                await _pedido.CambiarEstadoAsync(idPedido,(int)EstadoPedido.Preparacion);
            }
        }

        public async Task FinalizarAsync(int idOrden)
        {
            var orden =await _orden.FindByIdAsync(idOrden);

            if (orden == null)
            {
                throw new InvalidOperationException("Orden no encontrada.");
            }

            if (orden.EstadoOrden!=(int)EstadoOrden.EnProceso)
            {
                throw new InvalidOperationException("La orden debe estar en proceso antes de finalizarse.");
            }

            orden.EstadoOrden =(int)EstadoOrden.Finalizada;
            orden.HoraFin =TimeOnly.FromDateTime(DateTime.Now);
            await _orden.SaveChangesAsync();
            int idPedido =orden.IdDetallePedidoNavigation.IdPedido;
            var ordenesPedido =await _orden.GetByPedidoAsync(idPedido);
            bool todasFinalizadas =ordenesPedido.Any()&&ordenesPedido.All(o =>o.EstadoOrden==(int)EstadoOrden.Finalizada);

            if (todasFinalizadas)
            {                
                await _pedido.CambiarEstadoAsync(idPedido,(int)EstadoPedido.Entregada);
            }
            else
            {
                var pedido =await _pedido.FindSimpleByIdAsync(idPedido);

                if (pedido != null&&pedido.EstadoPedido!=(int)EstadoPedido.Procesando)
                {
                    await _pedido.CambiarEstadoAsync(idPedido,(int)EstadoPedido.Procesando);
                }
            }
        }

        private async Task ValidarPasoAnteriorAsync(Orden ordenActual)
        {
            var preparaciones =await _preparacion.GetByProductoAsync(ordenActual.IdProducto);

            var actual =preparaciones.FirstOrDefault(p =>p.IdEstacion==ordenActual.IdEstacion);

            if (actual == null)
            {
                throw new InvalidOperationException("No se encontró la configuración de preparación de esta orden.");
            }

            var pasosAnteriores = preparaciones.Where(p =>p.Orden<actual.Orden).ToList();

            if (!pasosAnteriores.Any())
                return;

            var ordenesPedido =await _orden.GetByPedidoAsync(ordenActual.IdDetallePedidoNavigation.IdPedido);

            foreach (var pasoAnterior in pasosAnteriores)
            {
                bool finalizado =ordenesPedido.Any(o =>o.IdDetallePedido==ordenActual.IdDetallePedido&&o.IdProducto==ordenActual.IdProducto&&o.IdEstacion==pasoAnterior.IdEstacion&&o.EstadoOrden==(int)EstadoOrden.Finalizada);

                if (!finalizado)
                {
                    throw new InvalidOperationException("Debe finalizar las estaciones anteriores antes de iniciar esta orden.");
                }
            }
        }
        //Hace lo mismo que el Enum pero es por fallas que se puedan dar
        private static string TextoEstadoOrden(EstadoOrden estado)
        {
            return estado switch
            {
                EstadoOrden.Pendiente =>"Pendiente",
                EstadoOrden.EnProceso =>"En proceso",
                EstadoOrden.Finalizada =>"Finalizada",
                _ =>estado.ToString()
            };
        }
    }
    //COMMIT DEL PROFESOR
}
