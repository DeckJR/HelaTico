using HelaTico.Application.DTOs;
using HelaTico.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HelaTico.Web.Controllers
{
    public class MenuController : Controller
    {
        private readonly IServiceMenu _serviceMenu;
        private readonly IServiceProducto _serviceProducto;
        private readonly IServiceCombo _serviceCombo;

        public MenuController(
            IServiceMenu serviceMenu,
            IServiceProducto serviceProducto,
            IServiceCombo serviceCombo)
        {
            _serviceMenu = serviceMenu;
            _serviceProducto = serviceProducto;
            _serviceCombo = serviceCombo;
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
            var menu = await _serviceMenu.GetMenusDisponiblesAsync();
            // Si no hay menú activo hoy, la vista recibe null y muestra un aviso
            return View(menu);
        }

        // GET: MenuController/Create
        public async Task<IActionResult> Create()
        {
            await CargarListas();
            return View();
        }

        // POST: MenuController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuDTO dto)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));
                await CargarListas();
                ViewBag.ErrorMessage = errors;
                return View(dto);
            }
            try
            {
                await _serviceMenu.AddAsync(dto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                await CargarListas();
                ViewBag.ErrorMessage = ex.Message;
                return View(dto);
            }
        }

        // GET: MenuController/Update/5
        public async Task<IActionResult> Update(int id)
        {
            var @object = await _serviceMenu.FindByIdAsync(id);
            await CargarListas(@object.IdProducto, @object.IdCombo);
            return View(@object);
        }

        // POST: MenuController/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, MenuDTO dto)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));
                await CargarListas(dto.IdProducto, dto.IdCombo);
                ViewBag.ErrorMessage = errors;
                return View(dto);
            }
            try
            {
                await _serviceMenu.UpdateAsync(id, dto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                await CargarListas(dto.IdProducto, dto.IdCombo);
                ViewBag.ErrorMessage = ex.Message;
                return View(dto);
            }
        }

        private async Task CargarListas(
            int[]? idsProductosSeleccionados = null,
            int[]? idsCombosSeleccionados = null)
        {
            ViewBag.ListProductos = new MultiSelectList(
                await _serviceProducto.ListAsync(), "IdProducto", "Nombre",
                idsProductosSeleccionados);

            ViewBag.ListCombos = new MultiSelectList(
                await _serviceCombo.ListAsync(), "IdCombo", "Nombre",
                idsCombosSeleccionados);
        }
    }
}