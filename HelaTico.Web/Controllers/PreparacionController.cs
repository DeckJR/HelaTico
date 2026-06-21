using HelaTico.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HelaTico.Web.Controllers
{
    public class PreparacionController : Controller
    {
        private readonly IServicePreparacion _servicePreparacion;

        public PreparacionController(IServicePreparacion servicePreparacion)
        {
            _servicePreparacion = servicePreparacion;
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
    }
}
