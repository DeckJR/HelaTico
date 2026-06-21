using HelaTico.Application.DTOs;
using HelaTico.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace HelaTico.Web.Controllers
{
    public class MenuController : Controller
    {
        private readonly IServiceMenu _serviceMenu;

        public MenuController(IServiceMenu serviceMenu)
        {
            _serviceMenu = serviceMenu;
        }

        public async Task<ActionResult> Index()
        {
            var lista = await _serviceMenu.ListAsync();
            return View(lista);
        }

        public async Task<ActionResult> Details(int id)
        {
            var @object = await _serviceMenu.FindByIdAsync(id);
            return View(@object);
        }

        public async Task<ActionResult> MenusDisponibles()
        {
            var lista = await _serviceMenu.GetMenusDisponiblesAsync();
            return View(lista);
        }
    }
}
