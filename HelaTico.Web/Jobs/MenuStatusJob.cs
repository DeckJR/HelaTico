using HelaTico.Application.Jobs.Interfaces;
using HelaTico.Application.Services.Interfaces;

namespace HelaTico.Web.Jobs
{
    public class MenuStatusJob : IMenuStatusJob
    {
        private readonly IServiceMenu _serviceMenu;
        private readonly ILogger<MenuStatusJob> _logger;

        public MenuStatusJob(IServiceMenu serviceMenu, ILogger<MenuStatusJob> logger)
        {
            _serviceMenu = serviceMenu;
            _logger = logger;
        }

        public async Task EjecutarAsync()
        {
            _logger.LogInformation("MenuStatusJob ejecutado: {Hora}", DateTime.Now);
            await _serviceMenu.ActualizarEstadoMenuAsync();
        }
    }
}