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
    }
}
