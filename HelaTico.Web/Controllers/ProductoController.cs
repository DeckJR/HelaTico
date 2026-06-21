using HelaTico.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HelaTico.Web.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IServiceProducto _serviceProducto;

        public ProductoController(IServiceProducto serviceProducto)
        { 
            _serviceProducto = serviceProducto;
        
        
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
    }
}
