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

        // GET: ProcesoPreparacionController
        public async Task<ActionResult> Index()
        {
            var lista = await _servicePreparacion.ListAsync();
            return View(lista);
        }

        // GET: ProcesoPreparacionController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var @object = await _servicePreparacion.FindByIdAsync(id);
            return View(@object);
        }
    }
}
