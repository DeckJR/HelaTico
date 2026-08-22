using HelaTico.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace HelaTico.Web.Controllers
{
    [Authorize(Roles = "Cocina")]
    public class OrdenController : Controller
    {
        private readonly IServiceOrden _serviceOrden;
        private readonly IServiceEstacion _serviceEstacion;

        public OrdenController(
            IServiceOrden serviceOrden,
            IServiceEstacion serviceEstacion)
        {
            _serviceOrden = serviceOrden;
            _serviceEstacion = serviceEstacion;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var estaciones = await _serviceOrden.ObtenerEstacionesAsync();
            return View(estaciones);
        }

        [HttpGet]
        public async Task<IActionResult> Estacion(int id)
        {
            var estaciones = await _serviceEstacion.ListAsync();
            var estacion = estaciones.FirstOrDefault(e => e.IdEstacion == id);

            if (estacion == null)
                return NotFound();

            var ordenes = await _serviceOrden.ObtenerPorEstacionAsync(id);

            ViewBag.IdEstacion = id;
            ViewBag.NombreEstacion = estacion.Descripcion;

            return View(ordenes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Iniciar(int id)
        {
            try
            {
                await _serviceOrden.IniciarAsync(id);
                return Json(new { exito = true, mensaje = "Orden iniciada correctamente." });
            }
            catch (System.Exception ex)
            {
                return Json(new { exito = false, mensaje = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Finalizar(int id)
        {
            try
            {
                await _serviceOrden.FinalizarAsync(id);
                return Json(new { exito = true, mensaje = "Orden finalizada correctamente." });
            }
            catch (System.Exception ex)
            {
                return Json(new { exito = false, mensaje = ex.Message });
            }
        }
    }
}