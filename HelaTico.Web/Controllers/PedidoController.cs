using HelaTico.Application.DTOs;
using HelaTico.Application.Services.Interfaces;
using HelaTico.Web.Helpers;
using HelaTico.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HelaTico.Web.Controllers
{
    [Authorize]
    public class PedidoController : Controller
    {
        private readonly IServicePedido _servicePedido;
        private readonly IServiceUsuario _serviceUsuario;
        private readonly IServiceTipoEntrega _serviceTipoEntrega;
        private readonly IServiceFacturaPedido _serviceFacturaPedido;

        public PedidoController(IServicePedido servicePedido,IServiceUsuario serviceUsuario,IServiceTipoEntrega serviceTipoEntrega,IServiceFacturaPedido serviceFacturaPedido)
        {
            _servicePedido = servicePedido;
            _serviceUsuario = serviceUsuario;
            _serviceTipoEntrega = serviceTipoEntrega;
            _serviceFacturaPedido = serviceFacturaPedido;
        }

        private int IdUsuarioLogueado =>int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        private bool EsCliente =>User.IsInRole("Cliente");

        public IActionResult Historial()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> HistorialData(DateOnly? fecha,int? estado)
        {
            int idUsuarioLogueado =int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            bool esCliente =User.IsInRole("Cliente");

            var resultado =await _servicePedido.ObtenerHistorialAsync(idUsuarioLogueado,esCliente,fecha,estado);

            return Json(resultado);
        }
        public async Task<IActionResult> Detalle(int id)
        {
            var detalle =await _servicePedido.ObtenerDetalleAsync(id);

            if (detalle == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Cliente"))
            {
                int idLogueado =int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                var pedidoCliente = (await _servicePedido.ObtenerHistorialAsync(idLogueado,true,null,null)).Any(p => p.IdPedido == id);

                if (!pedidoCliente)
                {
                    return Forbid();
                }
            }
            return View(detalle);
        }

        [HttpGet]
        public async Task<IActionResult> Registrar()
        {
            var carrito =CarritoSessionHelper.Obtener(HttpContext.Session);

            if (!carrito.Any())
            {
                return RedirectToAction("Ver","Carrito");
            }

            var model =new PedidoRegistroViewModel
                {
                    Carrito =carrito,
                    TiposEntrega =await _serviceTipoEntrega.ListAsync(),
                    EsCliente =EsCliente
                };

            if (EsCliente)
            {
                model.ClienteSeleccionado = await _serviceUsuario.FindByIdAsync(IdUsuarioLogueado);
            }
            else
            {
                var encargado = await _serviceUsuario.FindByIdAsync(IdUsuarioLogueado);

                if (encargado != null)
                {
                    model.NombreEncargado =$"{encargado.Nombre} " +$"{encargado.Apellido1}";
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> BuscarClientes(string nombre)
        {
            var clientes = await _serviceUsuario.BuscarClientesAsync(nombre??string.Empty);

            var resultado =clientes.Select(u => new
                    {
                        u.IdUsuario,
                        u.Nombre,
                        u.Apellido1,
                        u.Correo
                    });

            return Json(resultado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(PedidoRegistroDTO dto)
        {
            var carrito = CarritoSessionHelper.Obtener(HttpContext.Session);

            try
            {                

                int idPedido =await _servicePedido.RegistrarPedidoAsync(dto,IdUsuarioLogueado,EsCliente,carrito);

                TempData["Success"] =$"Pedido #{idPedido} registrado. " +$"Complete el pago.";

                return RedirectToAction(nameof(Pago),new
                    {
                        id = idPedido
                    });
            }
            catch (Exception ex)
            {                
                var model =new PedidoRegistroViewModel
                    {
                        Carrito =carrito,
                        TiposEntrega =await _serviceTipoEntrega.ListAsync(),
                        EsCliente =EsCliente,
                        Dto =dto,
                        Error =ex.Message
                    };
                if (EsCliente)
                {
                    model.ClienteSeleccionado = await _serviceUsuario.FindByIdAsync(IdUsuarioLogueado);
                }
                else
                {
                    if (dto.IdCliente > 0)
                    {
                        model.ClienteSeleccionado =await _serviceUsuario.FindByIdAsync(dto.IdCliente);
                    }

                    var encargado =await _serviceUsuario.FindByIdAsync(IdUsuarioLogueado);

                    if (encargado != null)
                    {
                        model.NombreEncargado =$"{encargado.Nombre} " +$"{encargado.Apellido1}";
                    }
                }
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Pago(int id)
        {
            try
            {
                var model =await _servicePedido.PrepararPagoAsync(id);

                if (model == null)
                {
                    return NotFound();
                }

                if (EsCliente)
                {
                    var pedidosCliente =await _servicePedido.ObtenerHistorialAsync(IdUsuarioLogueado,true,null, null);

                    bool perteneceAlCliente =pedidosCliente.Any(p => p.IdPedido == id);

                    if (!perteneceAlCliente)
                    {
                        return Forbid();
                    }
                }

                return View(model);
            }
            catch (InvalidOperationException ex)
            {

                TempData["Error"] = ex.Message;


                return RedirectToAction(nameof(Detalle),new{id});
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pago(ProcesarPagoDTO dto)
        {
            try
            {

                await _servicePedido.ProcesarPagoAsync(dto);

                CarritoSessionHelper.Limpiar(HttpContext.Session);

                TempData["Success"] ="Pago procesado correctamente.";

                return RedirectToAction(nameof(Detalle),new{id = dto.IdPedido});
            }
            catch (Exception ex)
            {                
                try
                {                   
                    var pago =await _servicePedido.PrepararPagoAsync(dto.IdPedido);

                    if (pago != null)
                    {
                        dto.Total =pago.Total;
                    }
                }
                catch
                {                    
                }
                ViewBag.Error =ex.Message;

                return View(dto);
            }
        }
        [HttpGet]
        public async Task<IActionResult> FacturaPdf(int id)
        {
            var detalle =await _servicePedido.ObtenerDetalleAsync(id);

            if (detalle == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Cliente"))
            {
                var pedidosCliente = await _servicePedido.ObtenerHistorialAsync(IdUsuarioLogueado,true,null,null);

                bool perteneceAlCliente = pedidosCliente.Any(p =>p.IdPedido == id);

                if (!perteneceAlCliente)
                {
                    return Forbid();
                }
            }
            else if (!User.IsInRole("Administrador")&&!User.IsInRole("Encargado"))
            {
                return Forbid();
            }

            byte[] pdf = await _serviceFacturaPedido.GenerarFacturaAsync(id);

            return File(pdf,"application/pdf",$"Factura-Pedido-{id}.pdf");
        }
    }
}