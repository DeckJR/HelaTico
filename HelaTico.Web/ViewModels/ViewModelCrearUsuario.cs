using System.ComponentModel.DataAnnotations;

namespace HelaTico.Web.ViewModels
{
    public class ViewModelCrearUsuario
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El primer apellido es obligatorio")]
        public string Apellido1 { get; set; } = null!;

        [Required(ErrorMessage = "El segundo apellido es obligatorio")]
        public string Apellido2 { get; set; } = null!;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo inválido")]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        [DataType(DataType.Password)]
        public string Contrasenna { get; set; } = null!;

        [Required(ErrorMessage = "Debe seleccionar un rol")]
        public int IdRolUsuario { get; set; }
    }
}