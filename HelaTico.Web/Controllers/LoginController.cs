using HelaTico.Application.DTOs;
using HelaTico.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HelaTico.Application.Services.Interfaces;
using System.Security.Claims;

namespace HelaTico.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IServiceUsuario _serviceUsuario;
        public LoginController(IServiceUsuario serviceUsuario)
        {
            _serviceUsuario = serviceUsuario;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View(new ViewModelLogin());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ViewModelLogin model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var (usuario, error) = await _serviceUsuario.LoginAsync(model.Correo, model.Contrasenna);
            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, error ?? "Correo o contraseña incorrectos");
                return View(model);
            }

            HttpContext.Session.Clear();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido1}"),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim(ClaimTypes.Role, usuario.DescripcionRol ?? string.Empty)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties { IsPersistent = true });

            return usuario.DescripcionRol switch
            {
                "Cocina" => RedirectToAction("Index", "Orden"), 
                _ => RedirectToAction("Index", "Home")
            };
        }

        [HttpGet]
        public IActionResult Registro()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View(new ViewModelRegistro());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(ViewModelRegistro model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new UsuarioDTO
            {
                Nombre = model.Nombre,
                Apellido1 = model.Apellido1,
                Apellido2 = model.Apellido2,
                Correo = model.Correo
            };

            var (exito, mensaje) = await _serviceUsuario.RegistrarClienteAsync(dto, model.Contrasenna);

            if (!exito)
            {
                ModelState.AddModelError(string.Empty, mensaje);
                return View(model);
            }

            TempData["MensajeExito"] = mensaje;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult Forbidden()
        {
            return View();
        }
    }
}