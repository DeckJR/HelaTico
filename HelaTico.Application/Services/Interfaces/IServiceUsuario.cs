using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelaTico.Application.DTOs;

namespace HelaTico.Application.Services.Interfaces
{
    public interface IServiceUsuario
    {
        Task<(UsuarioDTO? Usuario, string? Error)> LoginAsync(string correo, string password);
        Task<UsuarioDTO?> FindByIdAsync(int id);
        Task<List<UsuarioDTO>> BuscarClientesAsync(string nombre);
        Task<List<UsuarioDTO>> ObtenerTodosAsync();
        Task<(bool Exito, string Mensaje)> RegistrarClienteAsync(UsuarioDTO dto, string password);
        Task<(bool Exito, string Mensaje)> CrearUsuarioAsync(UsuarioDTO dto, string password);
        Task<(bool Exito, string Mensaje)> ActualizarUsuarioAsync(UsuarioDTO dto);  
        Task<(bool Exito, string Mensaje)> CambiarEstadoAsync(int idUsuario, int nuevoEstado);
        Task<List<RolDTO>> ObtenerRolesAsync();
    }
}