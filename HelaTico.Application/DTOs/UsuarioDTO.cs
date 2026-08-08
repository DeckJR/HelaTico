using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelaTico.Application.DTOs
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido1 { get; set; } = null!;
        public string Apellido2 { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string? Contrasenna { get; set; }
        public int IdRolUsuario { get; set; }
        public string? DescripcionRol { get; set; }
        public int EstadoUsuario { get; set; }
    }
}
