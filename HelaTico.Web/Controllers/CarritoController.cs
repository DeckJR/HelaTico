using HelaTico.Application.Services.Interfaces;
using HelaTico.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace HelaTico.Web.Controllers
{
    public class CarritoController : Controller
    {
        private readonly IServiceCarrito _serviceCarrito;

        public CarritoController(
            IServiceCarrito serviceCarrito)
        {
            _serviceCarrito = serviceCarrito;
        }

        public IActionResult Ver()
        {
            var items =
                CarritoSessionHelper.Obtener(
                    HttpContext.Session);

            var resumen =
                _serviceCarrito.CalcularResumen(items);

            return View(resumen);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Agregar(
            string tipo,
            int id,
            string nombre,
            string precio,
            int cantidad,
            string imagenUrl)
        {
            if (cantidad < 1)
                cantidad = 1;

            if (!decimal.TryParse(
                precio,
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out decimal precioDecimal))
            {
                return Json(new
                {
                    exito = false,
                    mensaje = "Precio inválido."
                });
            }

            CarritoSessionHelper.AgregarItem(
                HttpContext.Session,
                tipo,
                id,
                nombre,
                precioDecimal,
                cantidad,
                imagenUrl);

            int total =
                CarritoSessionHelper
                    .ObtenerCantidadTotal(
                        HttpContext.Session);

            return Json(new
            {
                exito = true,
                cantidadTotal = total
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActualizarCantidad(
            string tipo,
            int id,
            int cantidad)
        {
            CarritoSessionHelper.ActualizarCantidad(
                HttpContext.Session,
                tipo,
                id,
                cantidad);

            var items =
                CarritoSessionHelper.Obtener(
                    HttpContext.Session);

            var resumen =
                _serviceCarrito.CalcularResumen(items);

            return Json(new
            {
                exito = true,
                cantidadTotal =
                    items.Sum(i => i.Cantidad),

                resumen.SubTotal,
                resumen.Impuesto,
                resumen.Total
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActualizarObservaciones(
            string tipo,
            int id,
            string observaciones)
        {
            CarritoSessionHelper
                .ActualizarObservaciones(
                    HttpContext.Session,
                    tipo,
                    id,
                    observaciones);

            return Json(new
            {
                exito = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(
            string tipo,
            int id)
        {
            CarritoSessionHelper.Eliminar(
                HttpContext.Session,
                tipo,
                id);

            var items =
                CarritoSessionHelper.Obtener(
                    HttpContext.Session);

            var resumen =
                _serviceCarrito.CalcularResumen(items);

            return Json(new
            {
                exito = true,
                cantidadTotal =
                    items.Sum(i => i.Cantidad),

                resumen.SubTotal,
                resumen.Impuesto,
                resumen.Total
            });
        }

        [HttpGet]
        public IActionResult Cantidad()
        {
            int total =
                CarritoSessionHelper
                    .ObtenerCantidadTotal(
                        HttpContext.Session);

            return Json(new
            {
                cantidadTotal = total
            });
        }
    }
}