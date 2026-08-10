using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelaTico.Infraestructure.Models;

namespace HelaTico.Application.Services.Interfaces
{
    public interface IServiceTipoCambio
    {
        // Retorna el tipo de cambio actual (compra/venta) desde Hacienda.
        
        Task<TipoCambio?> ObtenerTipoCambioAsync();

        // Convierte un monto en colones a dólares usando el tipo de cambio de venta.
        Task<decimal?> ConvertirADolaresAsync(decimal montoColones);
    }
}
