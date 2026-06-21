using HelaTico.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HelaTico.Web.Controllers
{
    public class ComboController : Controller
    {
        private readonly IServiceCombo _serviceCombo;

        public ComboController(IServiceCombo serviceCombo)
        {
            _serviceCombo = serviceCombo;
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
    }
}
