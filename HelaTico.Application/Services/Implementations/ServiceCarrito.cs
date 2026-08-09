using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelaTico.Application.DTOs;
using HelaTico.Application.Services.Interfaces;

namespace HelaTico.Application.Services.Implementations
{
    public class ServiceCarrito : IServiceCarrito
    {
        private const decimal TASA_IMPUESTO = 0.13m;

        public CarritoResumenDTO CalcularResumen(List<CarritoItemDTO> items)
        {
            decimal subTotal = items.Sum(i => i.Precio * i.Cantidad);
            decimal impuesto = Math.Round(subTotal * TASA_IMPUESTO, 2);
            decimal total = subTotal + impuesto;

            return new CarritoResumenDTO
            {
                Items = items,
                SubTotal = subTotal,
                Impuesto = impuesto,
                Total = total
            };
        }
    }
}