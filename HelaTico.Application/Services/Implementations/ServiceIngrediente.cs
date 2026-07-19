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
    public class ServiceIngrediente : IServiceIngrediente
    {
        private readonly IRepositoryIngrediente _repository;
        private readonly IMapper _mapper;
        public ServiceIngrediente(IRepositoryIngrediente repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IngredienteDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            return _mapper.Map<IngredienteDTO>(@object);
        }

        public async Task<ICollection<IngredienteDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            return _mapper.Map<ICollection<IngredienteDTO>>(list);
        }
    }
}