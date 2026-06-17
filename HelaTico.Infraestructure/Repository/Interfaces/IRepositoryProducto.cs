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

    }
}
