using HelaTico.Application.DTOs;
using HelaTico.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HelaTico.Web.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IServiceProducto _serviceProducto;
        private readonly IServiceCategoria _serviceCategoria;
        private readonly IServiceIngrediente _serviceIngrediente;

        public ProductoController(
            IServiceProducto serviceProducto,
            IServiceCategoria serviceCategoria,
            IServiceIngrediente serviceIngrediente)
        {
            _serviceProducto = serviceProducto;
            _serviceCategoria = serviceCategoria;
            _serviceIngrediente = serviceIngrediente;
        }

        public async Task<ActionResult> Index()
        {
            var lista = await _serviceProducto.ListAsync();
            return View(lista);
        }

        public async Task<ActionResult> Details(int id)
        {
            var @object = await _serviceProducto.FindByIdAsync(id);
            return View(@object);
        }

        public async Task<ActionResult> ProductoCards()
        {
            var lista = await _serviceProducto.ListAsync();
            return View(lista);
        }

        // GET: ProductoController/Create
        public async Task<IActionResult> Create()
        {
            await CargarListas();
            return View();
        }

        // POST: ProductoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoDTO dto, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                using var target = new MemoryStream();
                imageFile.OpenReadStream().CopyTo(target);
                dto.Imagen = target.ToArray();
            }

            if (!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                await CargarListas();
                ViewBag.ErrorMessage = errors;
                return View(dto);
            }

            try
            {
                await _serviceProducto.AddAsync(dto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                await CargarListas();
                ViewBag.ErrorMessage = ex.Message;
                return View(dto);
            }
        }

        // GET: ProductoController/Update/5
        public async Task<IActionResult> Update(int id)
        {
            var @object = await _serviceProducto.FindByIdAsync(id);
            await CargarListas(@object.IdCategoria, @object.IdIngrediente);
            return View(@object);
        }

        // POST: ProductoController/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, ProductoDTO dto, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                using var target = new MemoryStream();
                imageFile.OpenReadStream().CopyTo(target);
                dto.Imagen = target.ToArray();
            }
            else
            {
                var actual = await _serviceProducto.FindByIdAsync(id);
                dto.Imagen = actual.Imagen; 
            }

            if (!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                await CargarListas(dto.IdCategoria, dto.IdIngrediente);
                ViewBag.ErrorMessage = errors;
                return View(dto);
            }

            try
            {
                await _serviceProducto.UpdateAsync(id, dto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                await CargarListas(dto.IdCategoria, dto.IdIngrediente);
                ViewBag.ErrorMessage = ex.Message;
                return View(dto);
            }
        }

        private async Task CargarListas(int? idCategoriaSeleccionada = null, int[]? idsIngredientesSeleccionados = null)
        {
            ViewBag.ListCategorias = new SelectList(await _serviceCategoria.ListAsync(), "IdCategoria", "Descripcion", idCategoriaSeleccionada);
            ViewBag.ListIngredientes = new MultiSelectList(await _serviceIngrediente.ListAsync(), "IdIngrediente", "Descripcion", idsIngredientesSeleccionados);
        }
    }
}