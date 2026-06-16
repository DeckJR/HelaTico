using System;
using System.Collections.Generic;

namespace HelaTico.Infraestructure.Models;

public partial class RolUsuario
{
    public int IdRolUsuario { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Usuario> Usuario { get; set; } = new List<Usuario>();
}
