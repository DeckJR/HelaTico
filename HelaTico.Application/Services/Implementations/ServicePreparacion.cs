using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HelaTico.Application.DTOs;
using HelaTico.Application.Services.Interfaces;
using HelaTico.Infraestructure.Models;
using HelaTico.Infraestructure.Repository.Interfaces;

namespace HelaTico.Application.Services.Implementations
{
    public class ServicePreparacion : IServicePreparacion
    {
        private readonly IRepositoryPreparacion _repository;
        private readonly IMapper _mapper;

        public ServicePreparacion(IRepositoryPreparacion repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PreparacionDTO> FindByIdAsync(int idProducto)
        {
            var @object = await _repository.FindByIdAsync(idProducto);
            var objectMapped = _mapper.Map<PreparacionDTO>(@object);
            return objectMapped;
        }

        public async Task<ICollection<PreparacionDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<PreparacionDTO>>(list);
            return collection;
        }

        public async Task AddAsync(int idProducto, int[] estacionIds, int[] ordenes)
        {
            for (int i = 0; i < estacionIds.Length; i++)
            {
                var paso = new Preparacion
                {
                    IdProducto = idProducto,
                    IdEstacion = estacionIds[i],
                    Orden = ordenes[i]
                };
                await _repository.AddPreparacionAsync(paso);
            }
        }

        public async Task UpdateAsync(int idProducto, int[] estacionIds, int[] ordenes)
        {
            await _repository.DeleteByProductoAsync(idProducto);

            for (int i = 0; i < estacionIds.Length; i++)
            {
                var paso = new Preparacion
                {
                    IdProducto = idProducto,
                    IdEstacion = estacionIds[i],
                    Orden = ordenes[i]
                };
                await _repository.AddPreparacionAsync(paso);
            }
        }
    }
}
