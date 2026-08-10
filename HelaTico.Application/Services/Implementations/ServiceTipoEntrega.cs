using HelaTico.Application.DTOs;
using HelaTico.Application.Services.Interfaces;
using HelaTico.Infraestructure.Repository.Interfaces;

namespace HelaTico.Application.Services.Implementations
{
    public class ServiceTipoEntrega
        : IServiceTipoEntrega
    {
        private readonly IRepositoryTipoEntrega
            _repository;

        public ServiceTipoEntrega(
            IRepositoryTipoEntrega repository)
        {
            _repository = repository;
        }

        public async Task<List<TipoEntregaDTO>>
            ListAsync()
        {
            var lista =
                await _repository.ListAsync();

            return lista.Select(
                x => new TipoEntregaDTO
                {
                    IdTipoEntrega =
                        x.IdTipoEntrega,

                    Descripcion =
                        x.Descripcion
                })
                .ToList();
        }
    }
}