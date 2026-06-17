using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HelaTico.Application.DTOs;
using HelaTico.Application.Services.Interfaces;
using HelaTico.Infraestructure.Repository.Interfaces;

namespace HelaTico.Application.Services.Implementations
{
    public class ServiceProducto : IServiceProducto
    {
        private readonly IRepositoryProducto _repository;
        private readonly IMapper _mapper;
        public ServiceProducto(IRepositoryProducto repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ProductoDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<ProductoDTO>(@object);
            return objectMapped;
        }

        public async Task<ICollection<ProductoDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<ProductoDTO>>(list);
            return collection;
        }
    }
}
