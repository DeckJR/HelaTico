using HelaTico.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelaTico.Web.Controllers
{
    [Authorize]
    public class TipoCambioController : Controller
    {
        private readonly IServiceTipoCambio _serviceTipoCambio;

        public TipoCambioController(IServiceTipoCambio serviceTipoCambio)
        {
            _serviceTipoCambio = serviceTipoCambio;
        }

        // GET /TipoCambio/Actual
        // Retorna el tipo de cambio actual como JSON.
        [HttpGet]
        public async Task<IActionResult> Actual()
        {
            var tc = await _serviceTipoCambio.ObtenerTipoCambioAsync();

            if (tc == null)
            {
                return Json(new
                {
                    disponible = false,
                    mensaje = "Servicio de tipo de cambio no disponible en este momento."
                });
            }

            return Json(new
            {
                disponible = true,
                compra = tc.Compra,
                venta = tc.Venta,
                fecha = tc.Fecha
            });
        }

        // GET /TipoCambio/Convertir?monto=####
        // Convierte un monto en colones a dólares y lo retorna como JSON.
        [HttpGet]
        public async Task<IActionResult> Convertir(decimal monto)
        {
            if (monto <= 0)
                return BadRequest(new { error = "El monto debe ser mayor a cero." });

            var dolares = await _serviceTipoCambio.ConvertirADolaresAsync(monto);

            if (dolares == null)
            {
                return Json(new
                {
                    disponible = false,
                    mensaje = "Servicio de tipo de cambio no disponible."
                });
            }

            return Json(new
            {
                disponible = true,
                montoColones = monto,
                montoDolares = dolares,
                simbolo = "$"
            });
        }
    }
}
