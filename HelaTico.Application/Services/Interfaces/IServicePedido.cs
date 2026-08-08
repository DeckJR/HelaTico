using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelaTico.Application.DTOs;

namespace HelaTico.Application.Services.Interfaces
{
    public interface IServicePedido
    {
        Task<List<PedidoListaDTO>> ObtenerHistorialAsync(int idUsuarioLogueado, bool esCliente, DateOnly? fecha, int? estadoPedido);
        Task<PedidoDetalleDTO?> ObtenerDetalleAsync(int idPedido);
    }
}