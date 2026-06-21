using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelaTico.Infraestructure.Models;

namespace HelaTico.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryPreparacion
    {
        Task<ICollection<Producto>> ListAsync();
        Task<Producto> FindByIdAsync(int idProducto);
    }
}
