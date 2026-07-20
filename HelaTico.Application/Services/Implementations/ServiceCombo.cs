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
    public class ServiceCombo : IServiceCombo
    {
        private readonly IRepositoryCombo _repository;
        private readonly IMapper _mapper;

        public ServiceCombo(IRepositoryCombo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ComboDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<ComboDTO>(@object);
            return objectMapped;
        }

        public async Task<ICollection<ComboDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<ComboDTO>>(list);
            return collection;
        }

        public async Task AddAsync(ComboDTO dto, int[] productosIds, int[] cantidades)
        {
            var combo = new Combo
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                Imagen = dto.Imagen ?? Array.Empty<byte>(),
                Estado = dto.Estado
            };

            var idCombo = await _repository.AddAsync(combo);

            for (int i = 0; i < productosIds.Length; i++)
            {
                var cp = new ComboProducto
                {
                    IdCombo = idCombo,
                    IdProducto = productosIds[i],
                    CantidadProducto = cantidades[i]
                };
                await _repository.AddComboProductoAsync(cp);
            }
        }

        public async Task UpdateAsync(int id, ComboDTO dto, int[] productosIds, int[] cantidades)
        {
            var combo = await _repository.FindByIdAsync(id);

            combo.Nombre = dto.Nombre;
            combo.Descripcion = dto.Descripcion;
            combo.Precio = dto.Precio;
            combo.Estado = dto.Estado;

            if (dto.Imagen != null && dto.Imagen.Length > 0)
                combo.Imagen = dto.Imagen;

            await _repository.UpdateAsync();

            await _repository.DeleteComboProductosAsync(id);

            for (int i = 0; i < productosIds.Length; i++)
            {
                var cp = new ComboProducto
                {
                    IdCombo = id,
                    IdProducto = productosIds[i],
                    CantidadProducto = cantidades[i]
                };
                await _repository.AddComboProductoAsync(cp);
            }
        }
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
