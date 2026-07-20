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
        public async Task<int> AddAsync(ProductoDTO dto)
        {
            if (await _repository.ExisteNombreAsync(dto.Nombre, 0))  // 0 = no hay id que excluir, es un producto nuevo
                throw new Exception("Ya existe un producto con ese nombre");

            var entity = _mapper.Map<Producto>(dto);
            return await _repository.AddAsync(entity, dto.IdIngrediente);
        }

        public async Task UpdateAsync(int id, ProductoDTO dto)
        {
            var productoActual = await _repository.FindByIdAsync(id);
            if (!productoActual.Nombre.Trim().ToUpper().Equals(dto.Nombre.Trim().ToUpper()))
            {
                if (await _repository.ExisteNombreAsync(dto.Nombre, id))
                    throw new Exception("Ya existe un producto con ese nombre");
            }

            var entity = _mapper.Map<Producto>(dto);
            entity.IdProducto = id;
            await _repository.UpdateAsync(entity, dto.IdIngrediente);
        }

        public async Task<bool> ExisteNombreAsync(string nombre, int id)
        {
            return await _repository.ExisteNombreAsync(nombre, id);
        }
    }
}
