using AutoMapper;
using HelaTico.Application.Config;
using HelaTico.Application.DTOs;
using HelaTico.Application.Services.Interfaces;
using HelaTico.Application.Utils;
using HelaTico.Infraestructure.Models;
using HelaTico.Infraestructure.Repository.Interfaces;
using Microsoft.Extensions.Options;

namespace HelaTico.Application.Services.Implementations
{
    public class ServiceUsuario : IServiceUsuario
    {
        private const int ESTADO_ACTIVO = 1;
        private const string ROL_CLIENTE = "Cliente";

        private readonly IRepositoryUsuario _repository;
        private readonly IMapper _mapper;
        private readonly IOptions<AppConfig> _options;

        public ServiceUsuario(IRepositoryUsuario repository, IMapper mapper, IOptions<AppConfig> options)
        {
            _repository = repository;
            _mapper = mapper;
            _options = options;
        }

        public async Task<(UsuarioDTO? Usuario, string? Error)> LoginAsync(string correo, string password)
        {
            var usuario = await _repository.FindByCorreoAsync(correo);

            string secret = _options.Value.Crypto.Secret;
            string passwordEncriptado = Cryptography.Encrypt(password, secret);

            if (usuario == null || usuario.Contrasenna != passwordEncriptado)
                return (null, "Correo o contraseña incorrectos");

            if (usuario.EstadoUsuario != ESTADO_ACTIVO)
                return (null, "Esta cuenta se encuentra desactivada. Contacte al administrador.");

            return (_mapper.Map<UsuarioDTO>(usuario), null);
        }

        public async Task<UsuarioDTO?> FindByIdAsync(int id)
        {
            var usuario = await _repository.FindByIdAsync(id);
            return usuario == null ? null : _mapper.Map<UsuarioDTO>(usuario);
        }

        public async Task<List<UsuarioDTO>> BuscarClientesAsync(string nombre)
        {
            var usuarios = await _repository.SearchClientesAsync(nombre);
            return _mapper.Map<List<UsuarioDTO>>(usuarios);
        }

        public async Task<List<UsuarioDTO>> ObtenerTodosAsync()
        {
            var usuarios = await _repository.GetAllAsync();
            return _mapper.Map<List<UsuarioDTO>>(usuarios);
        }

        public async Task<(bool Exito, string Mensaje)> RegistrarClienteAsync(UsuarioDTO dto, string password)
        {
            if (await _repository.ExistsByCorreoAsync(dto.Correo))
                return (false, "Ya existe una cuenta registrada con ese correo.");

            var roles = await _repository.GetRolesAsync();
            var rolCliente = roles.FirstOrDefault(r => r.Descripcion == ROL_CLIENTE);
            if (rolCliente == null)
                return (false, "No se encontró el rol Cliente configurado en el sistema.");

            string secret = _options.Value.Crypto.Secret;

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido1 = dto.Apellido1,
                Apellido2 = dto.Apellido2,
                Correo = dto.Correo,
                Contrasenna = Cryptography.Encrypt(password, secret),
                IdRolUsuario = rolCliente.IdRolUsuario,
                EstadoUsuario = ESTADO_ACTIVO
            };

            await _repository.CreateAsync(usuario);
            return (true, "Cuenta creada correctamente. Ya podés iniciar sesión.");
        }

        public async Task<(bool Exito, string Mensaje)> CrearUsuarioAsync(UsuarioDTO dto, string password)
        {
            if (await _repository.ExistsByCorreoAsync(dto.Correo))
                return (false, "Ya existe un usuario registrado con ese correo.");

            string secret = _options.Value.Crypto.Secret;

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido1 = dto.Apellido1,
                Apellido2 = dto.Apellido2,
                Correo = dto.Correo,
                Contrasenna = Cryptography.Encrypt(password, secret),
                IdRolUsuario = dto.IdRolUsuario,
                EstadoUsuario = ESTADO_ACTIVO
            };

            await _repository.CreateAsync(usuario);
            return (true, "Usuario creado correctamente.");
        }

        public async Task<(bool Exito, string Mensaje)> CambiarEstadoAsync(int idUsuario, int nuevoEstado)
        {
            var usuario = await _repository.FindByIdAsync(idUsuario);
            if (usuario == null)
                return (false, "El usuario no existe.");

            usuario.EstadoUsuario = nuevoEstado;
            await _repository.UpdateAsync(usuario);
            return (true, "Estado actualizado correctamente.");
        }

        public async Task<List<RolDTO>> ObtenerRolesAsync()
        {
            var roles = await _repository.GetRolesAsync();
            return roles
                .Where(r => r.Descripcion != ROL_CLIENTE)
                .Select(r => new RolDTO { IdRolUsuario = r.IdRolUsuario, Descripcion = r.Descripcion })
                .ToList();
        }
    }
}