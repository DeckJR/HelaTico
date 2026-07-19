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
    public class ServiceMenu : IServiceMenu
    {
        private readonly IRepositoryMenu _repository;
        private readonly IMapper _mapper;
        public ServiceMenu(IRepositoryMenu repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<MenuDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<MenuDTO>(@object);
            return objectMapped;
        }
        public async Task<ICollection<MenuDTO>> GetMenusDisponiblesAsync()
        {
            var list = await _repository.GetMenusDisponiblesAsync();
            var collection = _mapper.Map<ICollection<MenuDTO>>(list);
            return collection;
        }
        public async Task<ICollection<MenuDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<MenuDTO>>(list);
            return collection;
        }

        public async Task<int> AddAsync(MenuDTO dto)
        {
            if (dto.FechaInicio > dto.FechaFinal)
                throw new Exception("La fecha de inicio no puede ser mayor a la fecha final");

            if (await _repository.ExisteNombreAsync(dto.Nombre))
                throw new Exception("Ya existe un menú con ese nombre");

            var entity = _mapper.Map<Menu>(dto);
            return await _repository.AddAsync(entity, dto.IdProducto, dto.IdCombo);
        }

        public async Task UpdateAsync(int id, MenuDTO dto)
        {
            if (dto.FechaInicio > dto.FechaFinal)
                throw new Exception("La fecha de inicio no puede ser mayor a la fecha final");

            var menuActual = await _repository.FindByIdAsync(id);
            if (!menuActual.Nombre.Trim().ToUpper().Equals(dto.Nombre.Trim().ToUpper()))
            {
                if (await _repository.ExisteNombreAsync(dto.Nombre))
                    throw new Exception("Ya existe un menú con ese nombre");
            }

            var entity = _mapper.Map<Menu>(dto);
            entity.IdMenu = id;
            await _repository.UpdateAsync(entity, dto.IdProducto, dto.IdCombo);
        }

        public async Task<bool> ExisteNombreAsync(string nombre, int idMenuExcluir = 0)
        {
            return await _repository.ExisteNombreAsync(nombre, idMenuExcluir);
        }
    }
}