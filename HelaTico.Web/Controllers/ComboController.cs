using HelaTico.Application.DTOs;
using HelaTico.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HelaTico.Web.Controllers
{
    public class ComboController : Controller
    {
        private readonly IServiceCombo _serviceCombo;
        private readonly IServiceProducto _serviceProducto;

        public ComboController(IServiceCombo serviceCombo, IServiceProducto serviceProducto)
        {
            _serviceCombo = serviceCombo;
            _serviceProducto = serviceProducto;
        }

        public async Task<ActionResult> ComboCards()
        {
            var lista = await _serviceCombo.ListAsync();
            return View(lista);
        }

        public async Task<ActionResult> Index()
        {
            var lista = await _serviceCombo.ListAsync();
            return View(lista);
        }

        public async Task<ActionResult> Details(int id)
        {
            var @object = await _serviceCombo.FindByIdAsync(id);
            return View(@object);
        }

        public async Task<ActionResult> Create()
        {
            ViewBag.ListaProductos = await _serviceProducto.ListAsync();
            return View(new ComboDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            ComboDTO dto,
            IFormFile? imagenFile,
            int[]? productosSeleccionados)
        {
            if (imagenFile != null && imagenFile.Length > 0)
            {
                using var ms = new MemoryStream();
                await imagenFile.CopyToAsync(ms);
                dto = dto with { Imagen = ms.ToArray() };
            }

            if (productosSeleccionados == null || productosSeleccionados.Length == 0)
                ModelState.AddModelError(string.Empty, "Debe seleccionar al menos un producto para el combo.");

            if (!ModelState.IsValid)
            {
                ViewBag.ListaProductos = await _serviceProducto.ListAsync();
                return View(dto);
            }

            var cantidades = productosSeleccionados!
                .Select(id =>
                {
                    var raw = Request.Form[$"cantidad_{id}"].FirstOrDefault();
                    return int.TryParse(raw, out var c) && c > 0 ? c : 1;
                })
                .ToArray();

            await _serviceCombo.AddAsync(dto, productosSeleccionados!, cantidades);
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Edit(int id)
        {
            var combo = await _serviceCombo.FindByIdAsync(id);
            ViewBag.ListaProductos = await _serviceProducto.ListAsync();
            ViewBag.ProductosDelCombo = combo.Productos; 
            return View(combo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            int id,
            ComboDTO dto,
            IFormFile? imagenFile,
            int[]? productosSeleccionados)
        {
            if (imagenFile != null && imagenFile.Length > 0)
            {
                using var ms = new MemoryStream();
                await imagenFile.CopyToAsync(ms);
                dto = dto with { Imagen = ms.ToArray() };
            }

            ModelState.Remove(nameof(dto.Imagen));

            if (productosSeleccionados == null || productosSeleccionados.Length == 0)
                ModelState.AddModelError(string.Empty, "Debe seleccionar al menos un producto para el combo.");

            if (!ModelState.IsValid)
            {
                ViewBag.ListaProductos = await _serviceProducto.ListAsync();
                var comboActual = await _serviceCombo.FindByIdAsync(id);
                ViewBag.ProductosDelCombo = comboActual.Productos;
                return View(dto);
            }

            var cantidades = productosSeleccionados!
                .Select(pid =>
                {
                    var raw = Request.Form[$"cantidad_{pid}"].FirstOrDefault();
                    return int.TryParse(raw, out var c) && c > 0 ? c : 1;
                })
                .ToArray();

            await _serviceCombo.UpdateAsync(id, dto, productosSeleccionados!, cantidades);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            await _serviceCombo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
