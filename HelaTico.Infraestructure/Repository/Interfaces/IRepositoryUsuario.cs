using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HelaTico.Infraestructure.Models;

namespace HelaTico.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryUsuario
    {
        Task<Usuario?> FindByCorreoAsync(string correo);
        Task<Usuario?> FindByIdAsync(int id);
        Task<List<Usuario>> SearchClientesAsync(string nombre);
        Task<List<Usuario>> GetAllAsync();
        Task<bool> ExistsByCorreoAsync(string correo);
        Task<int> CreateAsync(Usuario usuario);
        Task UpdateAsync(Usuario usuario);
        Task<List<RolUsuario>> GetRolesAsync();
    }
}