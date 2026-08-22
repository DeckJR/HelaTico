using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelaTico.Application.Services.Interfaces
{
    public interface IServiceFacturaPedido
    {
        Task<byte[]> GenerarFacturaAsync(
            int idPedido);
    }                   
}
