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
            // Si ya inició sesión, no tiene sentido mostrarle el formulario de login otra vez
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

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

            // Evitar que el carrito (u otros datos) de la sesión anterior
            // se "arrastre" a este nuevo usuario logeado.
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
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Limpiar el carrito y cualquier otro dato de sesión del usuario que sale
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