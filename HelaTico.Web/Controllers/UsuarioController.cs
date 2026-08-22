using HelaTico.Application.DTOs;
using HelaTico.Application.Services.Interfaces;
using HelaTico.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HelaTico.Web.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly IServiceUsuario _serviceUsuario;

        public UsuarioController(IServiceUsuario serviceUsuario)
        {
            _serviceUsuario = serviceUsuario;
        }


        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuarios = await _serviceUsuario.ObtenerTodosAsync();

            return View(usuarios);
        }


        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = await _serviceUsuario.ObtenerRolesAsync();

            return View(new ViewModelCrearUsuario());
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            ViewModelCrearUsuario model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles =
                    await _serviceUsuario.ObtenerRolesAsync();

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

            var (exito, mensaje) =
                await _serviceUsuario.CrearUsuarioAsync(
                    dto,
                    model.Contrasenna);

            if (!exito)
            {
                ModelState.AddModelError(
                    string.Empty,
                    mensaje);

                ViewBag.Roles =
                    await _serviceUsuario.ObtenerRolesAsync();

                return View(model);
            }

            TempData["MensajeExito"] = mensaje;

            return RedirectToAction("Index");
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var idUsuarioLogueado = ObtenerIdUsuarioLogueado();

            if (idUsuarioLogueado == null)
                return Forbid();

            bool esAdministrador =
                User.IsInRole("Administrador");

         
            if (!esAdministrador &&
                id != idUsuarioLogueado.Value)
            {
                return Forbid();
            }

            var usuario =
                await _serviceUsuario.FindByIdAsync(id);

            if (usuario == null)
                return NotFound();

            // Los roles solamente se cargan para el administrador.
            if (esAdministrador)
            {
                ViewBag.Roles =
                    await _serviceUsuario.ObtenerRolesAsync();
            }

            var model = new ViewModelEditarUsuario
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Apellido1 = usuario.Apellido1,
                Apellido2 = usuario.Apellido2,
                Correo = usuario.Correo,
                IdRolUsuario = usuario.IdRolUsuario,
                DescripcionRol = usuario.DescripcionRol
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            ViewModelEditarUsuario model)
        {
            var idUsuarioLogueado =
                ObtenerIdUsuarioLogueado();

            if (idUsuarioLogueado == null)
                return Forbid();

            bool esAdministrador =
                User.IsInRole("Administrador");

      

            if (!esAdministrador &&
                model.IdUsuario != idUsuarioLogueado.Value)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                if (esAdministrador)
                {
                    ViewBag.Roles =
                        await _serviceUsuario.ObtenerRolesAsync();
                }

                return View(model);
            }

            var usuarioActual =
                await _serviceUsuario.FindByIdAsync(
                    model.IdUsuario);

            if (usuarioActual == null)
                return NotFound();



            int idRolFinal;

            if (esAdministrador)
            {
                idRolFinal = model.IdRolUsuario;
            }
            else
            {
                idRolFinal =
                    usuarioActual.IdRolUsuario;
            }

            var dto = new UsuarioDTO
            {
                IdUsuario = usuarioActual.IdUsuario,

                Nombre = model.Nombre,

                Apellido1 = model.Apellido1,

                Apellido2 = model.Apellido2,

                Correo = model.Correo,

                IdRolUsuario = idRolFinal
            };

            var (exito, mensaje) =
                await _serviceUsuario.ActualizarUsuarioAsync(dto);

            if (!exito)
            {
                ModelState.AddModelError(
                    string.Empty,
                    mensaje);

                if (esAdministrador)
                {
                    ViewBag.Roles =
                        await _serviceUsuario.ObtenerRolesAsync();
                }

                return View(model);
            }

            TempData["MensajeExito"] = mensaje;

      
            if (esAdministrador)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction(
                "Edit",
                new
                {
                    id = usuarioActual.IdUsuario
                });
        }


        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(
            int id,
            int nuevoEstado)
        {
            var (exito, mensaje) =
                await _serviceUsuario.CambiarEstadoAsync(
                    id,
                    nuevoEstado);

            return Json(new
            {
                exito,
                mensaje
            });
        }

      
        private int? ObtenerIdUsuarioLogueado()
        {
            var claim =
                User.FindFirst(
                    ClaimTypes.NameIdentifier);

            if (claim == null)
                return null;

            if (!int.TryParse(
                    claim.Value,
                    out int idUsuario))
            {
                return null;
            }

            return idUsuario;
        }
    }
}