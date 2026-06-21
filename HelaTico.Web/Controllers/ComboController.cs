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

        // GET: ComboController
        public async Task<ActionResult> Index()
        {
            var lista = await _serviceCombo.ListAsync();
            return View(lista);
        }

        // GET: ComboController/Details/#
        public async Task<ActionResult> Details(int id)
        {
            var @object = await _serviceCombo.FindByIdAsync(id);
            return View(@object);
        }
    }
}
