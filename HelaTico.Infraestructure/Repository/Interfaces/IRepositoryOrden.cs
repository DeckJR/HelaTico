using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelaTico.Infraestructure.Models;

namespace HelaTico.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryOrden
    {
        Task AddRangeAsync(IEnumerable<Orden> ordenes);
        Task<List<Estacion>>GetEstacionesAsync();
        Task<List<Orden>>GetByEstacionAsync(int idEstacion);
        Task<Orden?>FindByIdAsync(int idOrden);
        Task<List<Orden>>GetByPedidoAsync(int idPedido);
        Task<bool>ExistenOrdenesPedidoAsync(int idPedido);
        Task SaveChangesAsync();
    }
}
