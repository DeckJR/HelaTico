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
    public class ServiceUsuario : IServiceUsuario
    {
        private const int ESTADO_ACTIVO = 1;

        private readonly IRepositoryUsuario _repository;
        private readonly IMapper _mapper;

        public ServiceUsuario(IRepositoryUsuario repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<(UsuarioDTO? Usuario, string? Error)> LoginAsync(string correo, string password)
        {
            var usuario = await _repository.FindByCorreoAsync(correo);

            if (usuario == null || usuario.Contrasenna != password)
                return (null, "Correo o contraseña incorrectos");

            if (usuario.EstadoUsuario != ESTADO_ACTIVO)
                return (null, "Esta cuenta se encuentra desactivada. Contacte al administrador.");

            return (_mapper.Map<UsuarioDTO>(usuario), null);
        }

        public async Task<UsuarioDTO?>FindByIdAsync(int id)
        {
            var usuario = await _repository.FindByIdAsync(id);

            return usuario == null ? null: _mapper.Map<UsuarioDTO>(usuario);
        }


        public async Task<List<UsuarioDTO>>BuscarClientesAsync(string nombre)
        {
            var usuarios = await _repository.SearchClientesAsync(nombre);

            return _mapper.Map<List<UsuarioDTO>>(usuarios);
        }
    }
}