using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using HelaTico.Infraestructure.Models;
using HelaTico.Infraestructure.Repository.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
 
namespace HelaTico.Infraestructure.Repository.Implementations
{
    public class RepositoryTipoCambio : IRepositoryTipoCambio
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<RepositoryTipoCambio> _logger;

        private const string CacheKey = "TipoCambio_Dolar";
        private const string UrlHacienda = "https://api.hacienda.go.cr/indicadores/tc/dolar";

        public RepositoryTipoCambio(
            HttpClient httpClient,
            IMemoryCache cache,
            ILogger<RepositoryTipoCambio> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
        }

        public async Task<TipoCambio?> ObtenerTipoCambioAsync()
        {
            //Se verifica si se consulto el tipo de cambio en la última hora
            //si es así se devuelve el valor del tipo de cambio que está en el cache
            if (_cache.TryGetValue(CacheKey, out TipoCambio? cached))
                return cached;

            try
            {
                var response = await _httpClient.GetAsync(UrlHacienda);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                var raw = JsonSerializer.Deserialize<HaciendaResponse>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (raw?.Venta == null || raw?.Compra == null)
                    return null;

                var dto = new TipoCambio
                {
                    Compra = raw.Compra.Valor,
                    Venta = raw.Venta.Valor,
                    Fecha = raw.Venta.FechaIndicador
                };

                // Guardar en caché por 1 hora para que cuando se vuelva a consultar
                //el tipo de cambio no se vuelva a solicitar la información a Hacienda
                _cache.Set(CacheKey, dto, TimeSpan.FromHours(1));

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar tipo de cambio de Hacienda.");
                return null;
            }
        }

        private class HaciendaResponse
        {
            [JsonPropertyName("compra")]
            public HaciendaIndicador? Compra { get; set; }

            [JsonPropertyName("venta")]
            public HaciendaIndicador? Venta { get; set; }
        }

        private class HaciendaIndicador
        {
            [JsonPropertyName("valor")]
            public decimal Valor { get; set; }

            [JsonPropertyName("fechaIndicador")]
            public string FechaIndicador { get; set; } = string.Empty;
        }
    }
}
