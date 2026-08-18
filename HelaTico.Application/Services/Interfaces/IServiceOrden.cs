using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelaTico.Application.DTOs;

namespace HelaTico.Application.Services.Interfaces
{
    public interface IServiceOrden
    {
        Task GenerarOrdenesPedidoAsync(int idPedido);
        Task<List<EstacionProcesoDTO>>ObtenerEstacionesAsync();
        Task<List<OrdenEstacionDTO>>ObtenerPorEstacionAsync(int idEstacion);
        Task IniciarAsync(int idOrden);
        Task FinalizarAsync(int idOrden);
    }
}
