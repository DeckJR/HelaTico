using HelaTico.Application.DTOs;
using HelaTico.Application.Services.Interfaces;
using HelaTico.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelaTico.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuarioController : Controller
    {
        private readonly IServiceUsuario _serviceUsuario;

        public UsuarioController(IServiceUsuario serviceUsuario)
        {
            _serviceUsuario = serviceUsuario;
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = await _serviceUsuario.ObtenerTodosAsync();
            return View(usuarios);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = await _serviceUsuario.ObtenerRolesAsync();
            return View(new ViewModelCrearUsuario());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ViewModelCrearUsuario model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await _serviceUsuario.ObtenerRolesAsync();
                return View(model);
            }

            var dto = new UsuarioDTO
            {
                Nombre = model.Nombre,
                Apellido1 = model.Apellido1,
                Apellido2 = model.Apellido2,
                Correo = model.Correo,
                IdRolUsuario = model.IdRolUsuario
            };

            var (exito, mensaje) = await _serviceUsuario.CrearUsuarioAsync(dto, model.Contrasenna);

            if (!exito)
            {
                ModelState.AddModelError(string.Empty, mensaje);
                ViewBag.Roles = await _serviceUsuario.ObtenerRolesAsync();
                return View(model);
            }

            TempData["MensajeExito"] = mensaje;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id, int nuevoEstado)
        {
            var (exito, mensaje) = await _serviceUsuario.CambiarEstadoAsync(id, nuevoEstado);
            return Json(new { exito, mensaje });
        }
    }
}