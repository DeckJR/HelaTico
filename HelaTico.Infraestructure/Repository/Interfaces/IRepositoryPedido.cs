using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelaTico.Infraestructure.Models;

namespace HelaTico.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryPedido
    {
        Task<List<Pedido>> GetHistorialClienteAsync(int idCliente, DateOnly? fecha, int? estadoPedido);
        Task<List<Pedido>> GetHistorialTodosAsync(DateOnly? fecha, int? estadoPedido);
        Task<Pedido?> FindByIdAsync(int id);
        Task<int> RegistrarPedidoAsync (Pedido pedido);
        Task<Pedido?>FindSimpleByIdAsync(int idPedido);
        Task AddPagoAsync(Pago pago);
        Task CambiarEstadoAsync(int idPedido,int nuevoEstado);
    }
}
