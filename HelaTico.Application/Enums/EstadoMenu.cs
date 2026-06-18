using System.ComponentModel.DataAnnotations;
namespace HelaTico.Application.Enums
{
    public enum EstadoMenu
    {
        [Display(Name = "Disponible")]
        Disponible = 1,

        [Display(Name = "No Disponible")]
        No_Disponible = 2,
    }

}
