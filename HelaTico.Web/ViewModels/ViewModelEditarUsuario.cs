using System.ComponentModel.DataAnnotations;

namespace HelaTico.Web.ViewModels
{
    public class ViewModelEditarUsuario
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El primer apellido es obligatorio")]
        public string Apellido1 { get; set; } = null!;

        [Required(ErrorMessage = "El segundo apellido es obligatorio")]
        public string Apellido2 { get; set; } = null!;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo inválido")]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = "Debe seleccionar un rol")]
        public int IdRolUsuario { get; set; }
        public string? DescripcionRol { get; set; }
    }
}