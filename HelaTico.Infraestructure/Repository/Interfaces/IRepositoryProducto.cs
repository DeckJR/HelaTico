using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelaTico.Infraestructure.Models;

namespace HelaTico.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryProducto
    {
        Task<ICollection<Producto>> ListAsync();
        Task<Producto> FindByIdAsync(int id);
        Task<int> AddAsync(Producto entity, int[] idsIngredientes);
        Task UpdateAsync(Producto entity, int[] idsIngredientes);
        Task<bool> ExisteNombreAsync(string nombre);
    }
}
