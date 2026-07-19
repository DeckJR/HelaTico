using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelaTico.Application.DTOs;
namespace HelaTico.Application.Services.Interfaces
{
    public interface IServiceMenu
    {
        Task<ICollection<MenuDTO>> ListAsync();
        Task<MenuDTO> FindByIdAsync(int id);
        Task<ICollection<MenuDTO>> GetMenusDisponiblesAsync();
        Task<int> AddAsync(MenuDTO dto);
        Task UpdateAsync(int id, MenuDTO dto);
        Task<bool> ExisteNombreAsync(string nombre, int idMenuExcluir = 0);
    }
}