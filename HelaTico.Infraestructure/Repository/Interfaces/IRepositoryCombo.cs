using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelaTico.Infraestructure.Models;

namespace HelaTico.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryCombo
    {
        Task<ICollection<Combo>> ListAsync();
        Task<Combo> FindByIdAsync(int id);
        Task<int> AddAsync(Combo entity);
        Task UpdateAsync();
        Task DeleteComboProductosAsync(int idCombo);
        Task AddComboProductoAsync(ComboProducto comboProducto);
        Task DeleteAsync(int id);
    }
}
