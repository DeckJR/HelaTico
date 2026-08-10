using System.ComponentModel.DataAnnotations;

namespace HelaTico.Application.Enums
{
    public enum MetodoPago
    {
        [Display(Name = "Efectivo")]
        Efectivo = 1,

        [Display(Name = "Tarjeta de crédito")]
        Credito = 2,

        [Display(Name = "Tarjeta de débito")]
        Debito = 3
    }
}