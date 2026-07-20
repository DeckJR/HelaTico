using HelaTico.Application.DTOs;
using HelaTico.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HelaTico.Web.Controllers
{
    public class PreparacionController : Controller
    {
        private readonly IServicePreparacion _servicePreparacion;
        private readonly IServiceEstacion _serviceEstacion;
        private readonly IServiceProducto _serviceProducto;

        public PreparacionController(IServicePreparacion servicePreparacion, IServiceEstacion serviceEstacion, IServiceProducto serviceProducto)
        {
            _servicePreparacion = servicePreparacion;
            _serviceEstacion = serviceEstacion;
            _serviceProducto = serviceProducto;
        }

        public async Task<ActionResult> Index()
        {
            var lista = await _servicePreparacion.ListAsync();
            return View(lista);
        }

        public async Task<ActionResult> Details(int id)
        {
            var @object = await _servicePreparacion.FindByIdAsync(id);
            return View(@object);
        }

        public async Task<ActionResult> Create()
        {
            await CargarViewBagCreate();
            return View(new PreparacionDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            PreparacionDTO dto,
            int[]? estacionIds,
            int[]? ordenes)
        {
            if (estacionIds == null || estacionIds.Length == 0)
                ModelState.AddModelError(string.Empty, "Debe agregar al menos una estación al proceso.");

            if (!ModelState.IsValid)
            {
                await CargarViewBagCreate();
                return View(dto);
            }

            await _servicePreparacion.AddAsync(dto.IdProducto, estacionIds!, ordenes!);
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Edit(int id)
        {
            var proceso = await _servicePreparacion.FindByIdAsync(id);
            ViewBag.ListaEstaciones = await _serviceEstacion.ListAsync();
            ViewBag.PasosActuales = proceso.Pasos; 
            return View(proceso);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            int id,
            PreparacionDTO dto,
            int[]? estacionIds,
            int[]? ordenes)
        {
            ModelState.Remove(nameof(dto.IdProducto));

            if (estacionIds == null || estacionIds.Length == 0)
                ModelState.AddModelError(string.Empty, "Debe agregar al menos una estación al proceso.");

            if (!ModelState.IsValid)
            {
                var procesoActual = await _servicePreparacion.FindByIdAsync(id);
                ViewBag.ListaEstaciones = await _serviceEstacion.ListAsync();
                ViewBag.PasosActuales = procesoActual.Pasos;
                return View(dto);
            }

            await _servicePreparacion.UpdateAsync(id, estacionIds!, ordenes!);
            return RedirectToAction(nameof(Index));
        }

        private async Task CargarViewBagCreate()
        {
            var todosProductos = await _serviceProducto.ListAsync();
            var productosConProceso = (await _servicePreparacion.ListAsync())
                                        .Select(p => p.IdProducto)
                                        .ToHashSet();

            ViewBag.ProductosSinProceso = todosProductos
                .Where(p => !productosConProceso.Contains(p.IdProducto))
                .ToList();

            ViewBag.ListaEstaciones = await _serviceEstacion.ListAsync();
        }
    }
}
