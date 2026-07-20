using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelaTico.Infraestructure.Models;
namespace HelaTico.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryMenu
    {
        Task<ICollection<Menu>> ListAsync();
        Task<Menu> FindByIdAsync(int id);
        Task<Menu?> GetMenusDisponiblesAsync();
        Task<int> AddAsync(Menu entity, int[] idsProductos, int[] idsCombos);
        Task UpdateAsync(Menu entity, int[] idsProductos, int[] idsCombos);
        Task<bool> ExisteNombreAsync(string nombre, int idMenuExcluir = 0);
    }
}