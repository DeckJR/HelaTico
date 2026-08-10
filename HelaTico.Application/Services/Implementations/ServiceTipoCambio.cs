using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelaTico.Infraestructure.Models;
using HelaTico.Application.Services.Interfaces;
using HelaTico.Infraestructure.Repository.Interfaces;

namespace HelaTico.Application.Services.Implementations
{
    public class ServiceTipoCambio : IServiceTipoCambio
    {
        private readonly IRepositoryTipoCambio _repository;

        public ServiceTipoCambio(IRepositoryTipoCambio repository)
        {
            _repository = repository;
        }

        public async Task<TipoCambio?> ObtenerTipoCambioAsync()
        {
            return await _repository.ObtenerTipoCambioAsync();
        }

        public async Task<decimal?> ConvertirADolaresAsync(decimal montoColones)
        {
            var tc = await _repository.ObtenerTipoCambioAsync();
            if (tc == null || tc.Venta == 0) return null;

            // Usamos el tipo de cambio de VENTA ya que es el que aplica
            // cuando el cliente paga en dólares al negocio porque el de compra
            //sería si el clinete comprará dólares al negocio
            return Math.Round(montoColones / tc.Venta, 2);
        }
    }
}