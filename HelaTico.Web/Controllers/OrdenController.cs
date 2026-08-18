using HelaTico.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelaTico.Web.Controllers
{
    [Authorize]
    public class OrdenController: Controller
    {
        private readonly IServiceOrden _serviceOrden;

        public OrdenController(IServiceOrden serviceOrden)
        {
            _serviceOrden = serviceOrden;
        }

        [HttpGet]
        public async Task<IActionResult>Index()
        {
            var estaciones =await _serviceOrden.ObtenerEstacionesAsync();

            return View(estaciones);
        }

        [HttpGet]
        public async Task<IActionResult>Estacion(int id)
        {
            var ordenes =await _serviceOrden.ObtenerPorEstacionAsync(id);

            ViewBag.IdEstacion =id;

            ViewBag.NombreEstacion =ordenes.FirstOrDefault()?.Estacion??"Estación";

            return View(ordenes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Iniciar(int id)
        {
            try
            {
                await _serviceOrden.IniciarAsync(id);

                return Json(
                    new
                    {
                        exito = true,
                        mensaje ="Orden iniciada correctamente."
                    }
                );
            }
            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        exito = false,
                        mensaje = ex.Message
                    }
                );
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Finalizar(int id)
        {
            try
            {
                await _serviceOrden.FinalizarAsync(id);

                return Json(
                    new
                    {
                        exito = true,
                        mensaje ="Orden finalizada correctamente."
                    }
                );
            }
            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        exito = false,
                        mensaje = ex.Message
                    });
            }
        }
    }
}