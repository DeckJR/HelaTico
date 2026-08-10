using System.ComponentModel.DataAnnotations;

namespace HelaTico.Application.DTOs
{
    public class ProcesarPagoDTO
    {
        public int IdPedido { get; set; }
        public decimal Total { get; set; }
        [Range(1,3,ErrorMessage = "Debe seleccionar un método de pago.")]
        public int MetodoPago { get; set; }
        public decimal? MontoEfectivo { get; set; }
        public string? NumeroTarjeta { get; set; }
        public string? NombreTarjeta { get; set; }
        public string? VencimientoTarjeta { get; set; }
        public string? CvvTarjeta { get; set; }
    }
}