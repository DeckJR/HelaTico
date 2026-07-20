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
    public class ServiceEstacion : IServiceEstacion
    {
        private readonly IRepositoryEstacion _repository;
        private readonly IMapper _mapper;

        public ServiceEstacion(IRepositoryEstacion repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICollection<EstacionDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            return _mapper.Map<ICollection<EstacionDTO>>(list);
        }
    }
}
