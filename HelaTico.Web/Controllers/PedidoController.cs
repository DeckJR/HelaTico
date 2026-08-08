using HelaTico.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HelaTico.Web.Controllers
{
    [Authorize]
    public class PedidoController : Controller
    {
        private readonly IServicePedido _servicePedido;

        public PedidoController(IServicePedido servicePedido)
        {
            _servicePedido = servicePedido;
        }

        public IActionResult Historial()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> HistorialData(DateOnly? fecha, int? estado)
        {
            int idUsuarioLogueado = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            bool esCliente = User.IsInRole("Cliente");

            var resultado = await _servicePedido.ObtenerHistorialAsync(idUsuarioLogueado, esCliente, fecha, estado);

            return Json(resultado);
        }
    }
}