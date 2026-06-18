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
        Task<ICollection<Menu>> GetMenusDisponiblesAsync();
    }
}
