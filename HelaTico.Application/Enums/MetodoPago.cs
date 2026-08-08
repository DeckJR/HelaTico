using System.ComponentModel.DataAnnotations;

namespace HelaTico.Application.Enums
{
    public enum MetodoPago
    {
        [Display(Name = "Efectivo")]
        Efectivo = 1,

        [Display(Name = "Tarjeta")]
        Tarjeta = 2
    }
}
