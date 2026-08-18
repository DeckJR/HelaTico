using System.ComponentModel.DataAnnotations;

namespace HelaTico.Application.Enums
{
    public enum EstadoOrden
    {
        [Display(Name = "Pendiente")]
        Pendiente = 1,

        [Display(Name = "En proceso")]
        EnProceso = 2,

        [Display(Name = "Finalizada")]
        Finalizada = 3
    }

}