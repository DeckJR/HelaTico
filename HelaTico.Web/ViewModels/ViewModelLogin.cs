using System.ComponentModel.DataAnnotations;

namespace HelaTico.Web.ViewModels
{
    public class ViewModelLogin
    {
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo inválido")]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string Contrasenna { get; set; } = null!;
    }
}